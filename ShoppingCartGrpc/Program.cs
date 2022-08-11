using DiscountGrpc.Protos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoppingCartGrpc.Infra.DataProvider.Entity;
using ShoppingCartGrpc.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
// 5001 - Grpc Product
// 5002 - Grpc Shopping Cart
// 5003 - Grpc Discount
builder.Services.AddGrpc(o => o.EnableDetailedErrors = true);
builder.Services.AddGrpcClient<DiscountGrpcService.DiscountGrpcServiceClient>(o =>
{
    o.Address = new Uri(configuration["ShoppingCart:UrlDiscountGrpc"]);
});
builder.Services.AddSingleton(typeof(DiscountService));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<ShoppingCartContext>(options => options.UseInMemoryDatabase("ShoppingCartDb"));

builder.Services.AddAuthentication("Bearer").AddJwtBearer(o =>
{
    o.Authority = configuration["ShoppingCart:IdentityServerUrl"];
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();
await SeedDatabaseAsync(app);

async Task SeedDatabaseAsync(IHost host)
{
    using var scope = host.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();

    await ShoppingCartContextSeed.SeedAsync(context);
}

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<ShoppingCartService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
