using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using ProductGrpc.Protos;
using ProductWorkerService.Config;

namespace ProductWorkerService.Factory
{
    public class ProductFactory
    {
        private readonly ILogger<ProductFactory> _logger;
        private readonly ProductWorkerConfig _options;

        public ProductFactory(ILogger<ProductFactory> logger, IOptions<ProductWorkerConfig> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public AddProductRequest Create()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 5);
            var product = new ProductModel()
            {
                Name = $"{_options.ProductName}_{guid.ToUpper()}",
                Description = _options.ProductName,
                Price = Random.Shared.Next(800, 12500),
                Status = ProductStatus.Low,
                CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            return new AddProductRequest() { Product = product };
        }
    }
}
