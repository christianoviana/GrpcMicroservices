using ProductWorkerService;
using ProductWorkerService.Config;
using ProductWorkerService.Factory;

IHost host = Host.CreateDefaultBuilder(args)   
    .ConfigureServices((builder, services) =>
    {
        services.Configure<ProductWorkerConfig>(builder.Configuration.GetSection(ProductWorkerConfig.Name));
        services.AddHostedService<Worker>();
        services.AddTransient<ProductFactory>();
    })
    .Build();

await host.RunAsync();
