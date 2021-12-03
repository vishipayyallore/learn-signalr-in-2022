using Microsoft.Azure.Cosmos;
using NumberGeneratorWorkerService.Services;
using NumbersNotification.Core.Common;
using NumbersNotification.Core.Domain;

namespace NumberGeneratorWorkerService
{
    public class Worker : BackgroundService
    {

        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        private readonly CosmosClient _cosmosClient;
        private readonly Database _database;
        private readonly Container _container;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _cosmosClient = CreatCosmosDbClient(GetConfigurationValue(Constants.CosmosDB.AccountEndpoint),
                                                GetConfigurationValue(Constants.CosmosDB.AccountKey),
                                                Constants.CosmosDB.ApplicationName);

            _database = CreateCosmosDatabase(GetConfigurationValue(Constants.NotificationDataStore.DatabaseId));

            _container = CreateCosmosContainer(GetConfigurationValue(Constants.NotificationDataStore.ContainerId),
                                                GetConfigurationValue(Constants.NotificationDataStore.PartitionKey));
        }

        private static CosmosClient CreatCosmosDbClient(string accountEndpoint, string accountKey, string applicationName)
        {
            return new CosmosClient(accountEndpoint, accountKey,
                                                         new CosmosClientOptions() { ApplicationName = applicationName });
        }

        private Database CreateCosmosDatabase(string databaseId)
        {
            return _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId)
                            .GetAwaiter()
                            .GetResult();
        }

        private Container CreateCosmosContainer(string containerId, string partitionKey)
        {
            return _database.CreateContainerIfNotExistsAsync(containerId, partitionKey)
                            .GetAwaiter()
                            .GetResult();
        }

        private string GetConfigurationValue(string key)
        {
            return _configuration[key];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {

                var value = MeterReadingService.GetCurrentReading();

                try
                {
                    var reading = new Reading
                    {
                        Value = value
                    };

                    var response = await _container.CreateItemAsync(reading, new PartitionKey(reading.Company));
                }
                catch (CosmosException de)
                {
                    Exception baseException = de.GetBaseException();
                    Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e);
                }
                finally
                {
                    Console.WriteLine("End of demo, press any key to exit.");
                }

                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now} :: Value: {value}");
                await Task.Delay(5000, stoppingToken);
            }

        }

    }

}