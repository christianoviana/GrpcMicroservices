using DiscountGrpc.Protos;
using Grpc.Net.Client;
using static DiscountGrpc.Protos.DiscountGrpcService;

namespace ShoppingCartGrpc.Services
{
    public class DiscountService
    {
        private readonly DiscountGrpcServiceClient _discountGrpcService;
        public DiscountService(DiscountGrpcServiceClient discountGrpcService)
        {
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        } 

        public async Task<DiscountModel> GetDiscountAsync(string discountCode)
        {
            var request = new GetDiscountRequest() { DiscountCode = discountCode };
            return await _discountGrpcService.GetDiscountAsync(request);
        }
    }
}
