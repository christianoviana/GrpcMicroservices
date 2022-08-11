using Grpc.Core;
using IdentityModel.Client;
using ShoppingCartGrpc.Protos;
using static ShoppingCartGrpc.Protos.ShoppingCartGrpcService;

namespace ShoppingCartWorkerService.Service
{
    public class ShoppingCartService
    {
        private readonly ILogger<ShoppingCartService> _logger;
        private readonly ShoppingCartGrpcServiceClient _shoppingCartGrpcClient;
        private readonly IConfiguration _configuration;

        public ShoppingCartService(ILogger<ShoppingCartService> logger,
                                   ShoppingCartGrpcServiceClient shoppingCartGrpcClient,
                                   IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _shoppingCartGrpcClient = shoppingCartGrpcClient ?? throw new ArgumentException(nameof(shoppingCartGrpcClient));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        public async Task<ShoppingCartModel> GetOrCreateShoppingCart(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            _logger.LogInformation($"Get Token From Is4");
            var token = await GetTokenFromIs4();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {token}");

            try
            {
                _logger.LogInformation($"Get Shopping Cart ${username}");
                var shoppingCartRequest = new GetShoppingCartRequest() { Username = username };
                return await _shoppingCartGrpcClient.GetShoppingCartAsync(shoppingCartRequest, headers);
            }
            catch (RpcException rpcEx)
            {
                if (rpcEx.StatusCode.Equals(StatusCode.NotFound))
                {
                    _logger.LogInformation($"Adding Shopping Cart ${username}");

                    var shoppingCartModel = new ShoppingCartModel() { Username = username, Data = DateTime.Now.ToString() };
                    return await _shoppingCartGrpcClient.AddShoppingCartAsync(shoppingCartModel, headers);
                }

                _logger.LogError($"Error ${rpcEx}");

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ${ex}");

                throw;
            }
        }

        public async Task<AddShoppingCartItemResponse> AddShoppingCartItem(string username, IEnumerable<CartItemModel> items, string discountCode = "")
        {
            _logger.LogInformation($"Get Token From Is4");
            var token = await GetTokenFromIs4();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {token}");

            var addCartItems = _shoppingCartGrpcClient.AddShoppingCartItem(headers);

            foreach (var item in items)
            {
                var cartItem = new AddShoppingCartItemRequest() { CartItem = item, Username = username, DiscountCode = discountCode };
                await addCartItems.RequestStream.WriteAsync(cartItem);

                Task.Delay(200).Wait();
            }

            await addCartItems.RequestStream.CompleteAsync();
            var addShoppingCartItemResponse = await addCartItems;

            return addShoppingCartItemResponse;
        }

        private async Task<string> GetTokenFromIs4()
        {
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(_configuration["WorkerService:IdentityServerUrl"]);

                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return String.Empty;
                }

                var token = await client.RequestClientCredentialsTokenAsync
                    (
                        new ClientCredentialsTokenRequest()
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = "ShoppingCartClientId",
                            ClientSecret = "secret",
                            Scope = "ShoppingCartApi"
                        }
                    );

                if (token.IsError)
                {
                    Console.WriteLine(token.Error);
                    return String.Empty;
                }

                return token.AccessToken;
            }
        }
    }
}
