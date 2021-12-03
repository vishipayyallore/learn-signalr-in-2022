using Microsoft.Azure.Cosmos;
using NumbersChangeFeedProcessor;

string _endpointUrl = "<replace with your Account Endpoint URL>";
string _primaryKey = "<replace with your Account Key>";
string _databaseId = "ToDoList";
string _containerId = "numbers";

using (var client = new CosmosClient(_endpointUrl, _primaryKey))
{
    var db = client.GetDatabase(_databaseId);
    var container = db.GetContainer(_containerId);

    Container leaseContainer = await db.CreateContainerIfNotExistsAsync(id: "consoleLeases", partitionKeyPath: "/id", throughput: 400);

    var builder = container.GetChangeFeedProcessorBuilder("numbersChangedProcessor", (IReadOnlyCollection<Reading> input, CancellationToken cancellationToken) =>
    {
        Console.WriteLine(input.Count + " Changes Received");

        foreach (var doc in input)
        {
            Console.WriteLine($"{doc.Id} :: {doc.Company} :: {doc.Value}");
        }

        return Task.CompletedTask;
    });

    var processor = builder.WithInstanceName("changeFeedConsole").WithLeaseContainer(leaseContainer).Build();

    await processor.StartAsync();
    Console.WriteLine("Started Change Feed Processor");
    Console.WriteLine("Press any key to stop the processor...");

    Console.ReadKey();

    Console.WriteLine("Stopping Change Feed Processor");
    await processor.StopAsync();
}
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

