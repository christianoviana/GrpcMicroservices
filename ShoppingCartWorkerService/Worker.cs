using AutoMapper;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using ShoppingCartGrpc.Protos;
using ShoppingCartWorkerService.Config;
using ShoppingCartWorkerService.Service;

namespace ShoppingCartWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ShoppingCartConfig _config;
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly List<string> discounts;

        public Worker(ProductService productService, 
            ShoppingCartService shoppingCartService,
            ILogger<Worker> logger, 
            IMapper mapper,
            IOptions<ShoppingCartConfig> config)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _config = config.Value ?? throw new ArgumentException(nameof(config));

            discounts = new List<string>
            {
                "CODE_050_pqrstuv",
                "CODE_100_liytrgm",
                "CODE_200_nbgrtyl",
                "CODE_300_rfvdbns"
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                    var products = await _productService.GetAllProducts();

                    var shoppingCart = await _shoppingCartService.GetOrCreateShoppingCart(_config.UserName);
                    Console.WriteLine($"Products: {products}");
                    Console.WriteLine($"Shopping Cart: {shoppingCart}");

                    var cartItems = _mapper.Map<IEnumerable<CartItemModel>>(products);

                    var addShoppingCartItemResponse = await _shoppingCartService.AddShoppingCartItem(_config.UserName, cartItems, discounts[1]);
                    Console.WriteLine(addShoppingCartItemResponse);

                    await Task.Delay(_config.TaskInterval, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ex.: {ex}");
            }           
        }
    }
}