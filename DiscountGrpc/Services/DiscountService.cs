using DiscountGrpc.Infra.DataProvider.Entity;
using DiscountGrpc.Protos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace DiscountGrpc.Services
{
    public class DiscountService : DiscountGrpcService.DiscountGrpcServiceBase
    {
        private DiscountContext _context;
        private ILogger<DiscountService> _logger;

        public DiscountService(DiscountContext context, ILogger<DiscountService> logger)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public override async Task<DiscountModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(e => e.Code.Equals(request.DiscountCode, StringComparison.OrdinalIgnoreCase));

            if (discount == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with code {request.DiscountCode} was not found"));
            }

            _logger.LogInformation($"The discount code {discount.Code} has amount of {discount.Amount}");

            return new DiscountModel()
            {
                Id = discount.Id,
                DiscountCode = discount.Code,
                Amount = discount.Amount,
            };
        }
    }
}
