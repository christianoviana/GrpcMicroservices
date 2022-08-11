using Microsoft.Extensions.Options;
using ProductWorkerService.Config;
using ProductWorkerService.Factory;
using ProductWorkerService.Service;

namespace ProductWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ProductWorkerConfig _options;
        private readonly ProductFactory _productFactory;

        public Worker(ILogger<Worker> logger, IOptions<ProductWorkerConfig> options, ProductFactory productFactory)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _options = options.Value ?? throw new ArgumentException(nameof(_options));
            _productFactory = productFactory ?? throw new ArgumentException(nameof(_productFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var client = new ProductGrpcClient(_options.UrlProductGrpc);               

                var response = await client.AddProductAsync(_productFactory.Create());

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogDebug("Product Added: {product}", response.ToString());
                await Task.Delay(_options.Interval, stoppingToken);
            }
        }
    }
}