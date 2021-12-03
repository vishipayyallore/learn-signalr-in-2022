using Microsoft.Azure.Cosmos;
using NumberGeneratorWorkerService.Domain;

namespace NumberGeneratorWorkerService
{
    public class Worker : BackgroundService
    {

        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        // The name of the database and container we will create
        // TODO: Move these two into configuration file.
        const string databaseId = "ToDoList";
        const string containerId = "numbers";
        const string partitionKey = "/company";

        int value = 1;

        private readonly CosmosClient _cosmosClient;
        private readonly Database _database;
        private Container _container;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _cosmosClient = new CosmosClient(_configuration["CosmosDbConnectionStrings:AccountEndpoint"],
                                             _configuration["CosmosDbConnectionStrings:AccountKey"],
                                             new CosmosClientOptions() { ApplicationName = "NumberGeneratorWorkerService" });

            _database = _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).GetAwaiter().GetResult();

            _container = _database.CreateContainerIfNotExistsAsync(containerId, partitionKey).GetAwaiter().GetResult();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                value++;
                if (value > 5)
                {
                    value = 1;
                }

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

        //private async Task BasicOperations()
        //{
        //    this.client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["accountEndpoint"]), ConfigurationManager.AppSettings["accountKey"]);

        //    await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Users" });

        //    await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Users"), new DocumentCollection { Id = "WebCustomers" });

        //    Console.WriteLine("Database and collection validation complete");
        //}

    }
}