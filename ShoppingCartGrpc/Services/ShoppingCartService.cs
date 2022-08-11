using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ShoppingCartGrpc;
using ShoppingCartGrpc.Domain.Models;
using ShoppingCartGrpc.Infra.DataProvider.Entity;
using ShoppingCartGrpc.Protos;

namespace ShoppingCartGrpc.Services
{
    [Authorize]
    public class ShoppingCartService : ShoppingCartGrpcService.ShoppingCartGrpcServiceBase
    {
        private readonly ShoppingCartContext _context;
        private readonly DiscountService _discountService;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingCartService> _logger;
        public ShoppingCartService(ShoppingCartContext context, DiscountService discountService,
            IMapper mapper, ILogger<ShoppingCartService> logger)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _discountService = discountService ?? throw new ArgumentException(nameof(discountService));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public override async Task<ShoppingCartModel> GetShoppingCart(GetShoppingCartRequest request, ServerCallContext context)
        {
            var shoppingCart = await _context.ShoppingCarts.Include(p => p.Items).FirstOrDefaultAsync(sc => sc.UserName.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

            if (shoppingCart is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Shopping cart with name {request.Username} was not found"));
            }

            return _mapper.Map<ShoppingCartModel>(shoppingCart);
        }

        public override async Task<ShoppingCartModel> AddShoppingCart(ShoppingCartModel request, ServerCallContext context)
        {
            var hasShoppingCart = await _context.ShoppingCarts.AnyAsync(e => e.UserName.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

            if (hasShoppingCart)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, $"Shopping cart with name {request.Username} already exists"));
            }

            var shoppingCart = _mapper.Map<ShoppingCart>(request);

            var shoppingCartAdded = await _context.ShoppingCarts.AddAsync(shoppingCart);
            await _context.SaveChangesAsync();

            return _mapper.Map<ShoppingCartModel>(shoppingCartAdded.Entity);
        }

        public override async Task<AddShoppingCartItemResponse> AddShoppingCartItem(IAsyncStreamReader<AddShoppingCartItemRequest> requestStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                var shoppingCart = await _context.ShoppingCarts.Include(p => p.Items).FirstOrDefaultAsync(sc => sc.UserName.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

                if (shoppingCart is null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Shopping cart with name {request.Username} was not found"));
                }

                var cartItem =  shoppingCart.Items?.Where(sc => sc.ProductId.Equals(request.CartItem.ProductId))
                                                   .FirstOrDefault();

                if (cartItem is not null)
                {
                    cartItem.IncreaseQuantity();
                }
                else
                {
                    var newCartItem = _mapper.Map<CartItem>(request.CartItem);
                    newCartItem.SetShoppingCartId(shoppingCart.Id);

                    if (!string.IsNullOrEmpty(request.DiscountCode))
                    {
                        try
                        {
                            var discount = await _discountService.GetDiscountAsync(request.DiscountCode);
                            newCartItem.SetDiscount(discount.Amount);
                        }
                        catch (Exception)
                        {
                            //TODO: Discount not applied
                        }
                    }

                    shoppingCart.AddItem(newCartItem);
                }
            }
           
            var cartItemsAdded = await _context.SaveChangesAsync();

            var response = new AddShoppingCartItemResponse()
            {
                InsertCount = cartItemsAdded,
                Success = cartItemsAdded > 0
            };

            return response;
        }

        public override async Task<RemoveShoppingCartItemResponse> RemoveShoppingCartItem(RemoveShoppingCartItemRequest request, ServerCallContext context)
        {
            var shoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(sc => sc.UserName.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

            if (shoppingCart is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Shopping cart with name {request.Username} was not found"));
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(sc => sc.ProductId.Equals(request.CartItem.ProductId));

            if (shoppingCart.Items is null || cartItem is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Cart item with product id {request.CartItem.ProductId} was not found"));
            }
                       
            _context.CartItems.Remove(cartItem);
            var itemsRemovedCount = await _context.SaveChangesAsync();

            var response = new RemoveShoppingCartItemResponse()
            {
                Success = itemsRemovedCount > 0
            };

            return response;

        }
    }
}