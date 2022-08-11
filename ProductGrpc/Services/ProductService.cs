
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProductGrpc.Domain.Models;
using ProductGrpc.Infra.DataProvider.Entity;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductGrpcService.ProductGrpcServiceBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly ProductContext _productContext;

        public ProductService(ProductContext productContext, ILogger<ProductService> logger, IMapper mapper)
        {
            _productContext = productContext ?? throw new ArgumentException(nameof(productContext));   
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public override async Task GetAllProduct(GetAllProductRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
           var products = await _productContext.Products.ToListAsync();

            foreach (var product in products)
            {
                var productModel = _mapper.Map<ProductModel>(product);

                await responseStream.WriteAsync(productModel);
            }
        }

        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _productContext.Products.FindAsync(request.ProductId);

            if (product is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product {request.ProductId} was not found."));
            }

            var productModel = _mapper.Map<ProductModel>(product);

            return productModel;
        }

        public override async Task<ProductModel> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            var product = _mapper.Map<Product>(request.Product);

            await _productContext.Products.AddAsync(product);
            await _productContext.SaveChangesAsync();

            var productModel = _mapper.Map<ProductModel>(product);

            return productModel;
        }

        public override async Task<ProductModel> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            var product = _mapper.Map<Product>(request.Product);

            bool hasProduct = await _productContext.Products.AnyAsync(p => product.Id == request.Product.ProductId);

            if (!hasProduct)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product {request.Product.ProductId} was not found."));
            }

            _productContext.Entry(product).State = EntityState.Modified;
            await _productContext.SaveChangesAsync();

            var productModel = _mapper.Map<ProductModel>(product);

            return productModel;
        }

        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {           
            var product = await _productContext.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product {request.ProductId} was not found."));
            }

            _productContext.Products.Remove(product);
            var deleteCount = await _productContext.SaveChangesAsync();

            var response = new DeleteProductResponse() { Success = deleteCount > 0 };

            return response;
        }

        public override async Task<InsertBulkProductResponse> InsertBulkProduct(IAsyncStreamReader<ProductModel> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var product = _mapper.Map<Product>(requestStream.Current);
                await _productContext.Products.AddAsync(product);
            }

            var insertCount = await _productContext.SaveChangesAsync();

            var response = new InsertBulkProductResponse() 
            { 
                Success = insertCount > 0,
                InsertCount = insertCount
            };

            return response;
        }
    }
}
