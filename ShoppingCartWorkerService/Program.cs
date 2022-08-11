using ProductGrpc.Protos;
using ShoppingCartGrpc.Protos;
using ShoppingCartWorkerService;
using ShoppingCartWorkerService.Config;
using ShoppingCartWorkerService.Mapper;
using ShoppingCartWorkerService.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
        var configuration = host.Configuration;

        services.Configure<ShoppingCartConfig>(host.Configuration.GetSection(ShoppingCartConfig.NAME));
        services.AddGrpcClient<ProductGrpcService.ProductGrpcServiceClient>(o =>
        {
            o.Address = new Uri(configuration["WorkerService:ProductServerUrl"]);
        });
        services.AddGrpcClient<ShoppingCartGrpcService.ShoppingCartGrpcServiceClient>(o =>
        {
            o.Address = new Uri(configuration["WorkerService:ShoppingCartServerUrl"]);
        });
        services.AddSingleton<ShoppingCartService>();
        services.AddSingleton<ProductService>();
        services.AddAutoMapper(typeof(ProductMapper));

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
