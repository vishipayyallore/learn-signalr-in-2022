using NumberGeneratorWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config => { config.AddUserSecrets("42B3177E-1CA6-448F-8F9A-1294955F5337"); })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
