using Grpc.Core;
using ProductGrpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProductGrpc.Protos.ProductGrpcService;

namespace ShoppingCartWorkerService.Service
{
    public class ProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly ProductGrpcServiceClient _productGrpcClient;
        public ProductService(ILogger<ProductService> logger, ProductGrpcServiceClient productGrpcClient)
        {
            _logger = logger;
            _productGrpcClient = productGrpcClient;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            IList<ProductModel> products = new List<ProductModel>();
            var productsStream = _productGrpcClient.GetAllProduct(new GetAllProductRequest());

            await foreach(var product in productsStream.ResponseStream.ReadAllAsync()) 
            { 
                products.Add(product);
            }

            return products;
        }
    }
}
