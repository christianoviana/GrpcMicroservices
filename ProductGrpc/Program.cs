using Microsoft.EntityFrameworkCore;
using ProductGrpc.Infra.DataProvider.Entity;
using ProductGrpc.Mapper;
using ProductGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(o => o.EnableDetailedErrors = true);
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddDbContext<ProductContext>(options => options.UseInMemoryDatabase("ProductDb"));

var app = builder.Build();
await SeedDatabase(app);

static async Task SeedDatabase(IHost host)
{
    using var scope = host.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ProductContext>();

    await ProductContextSeed.SeedAsync(context);
}

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
