using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using ProductGrpc.Protos;

namespace ProductWorkerService.Service
{
    public class ProductGrpcClient
    {
        public string Url { get; }

        public ProductGrpcClient(string url)
        {
            Url = url;
        }

        public async Task<ProductModel> AddProductAsync(AddProductRequest request)
        {
            var channel = GrpcChannel.ForAddress(Url);
            var client = new ProductGrpcService.ProductGrpcServiceClient(channel);          

            var response = await client.AddProductAsync(request);

            return response;
        }
    }
}
