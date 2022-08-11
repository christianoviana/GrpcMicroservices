using DiscountGrpc.Infra.DataProvider.Entity;
using DiscountGrpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddDbContext<DiscountContext>(o => o.UseInMemoryDatabase("DiscountDb"));
builder.Services.AddGrpc();

var app = builder.Build();

await SeedDatabaseAsync(app);

async Task SeedDatabaseAsync(IHost app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();

    await DiscountContextSeed.SeedAsync(context);
}

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
