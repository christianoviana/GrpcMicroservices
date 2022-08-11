
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpc.Protos;

var address = "https://localhost:5001";
var channel = GrpcChannel.ForAddress(address);
var client = new ProductGrpcService.ProductGrpcServiceClient(channel);

Console.WriteLine("AddProduct");
await AddProduct(client);

Console.WriteLine("GetProductById");
await GetProductById(client);

Console.WriteLine("GetAllProducts");
await GetAllProducts(client);

Console.WriteLine("UpdateProduct");
await UpdateProduct(client);

Console.WriteLine("DeleteProductById");
await DeleteProductById(client);

Console.WriteLine("InsertBulkProduct");
await InsertBulkProduct(client);

Console.WriteLine("GetAllProducts");
await GetAllProducts(client);

Console.ReadKey();


static async Task GetProductById(ProductGrpcService.ProductGrpcServiceClient client)
{
    var product = await client.GetProductAsync(new GetProductRequest() { ProductId = 2 });

    Console.WriteLine(product);
}

static async Task GetAllProducts(ProductGrpcService.ProductGrpcServiceClient client)
{
    using (var products = client.GetAllProduct(new GetAllProductRequest()))
    {
        await foreach (var product in products.ResponseStream.ReadAllAsync(new CancellationToken()))
        {
            Console.WriteLine(product);
        }
    }
}

static async Task AddProduct(ProductGrpcService.ProductGrpcServiceClient client)
{
    var product = new ProductModel()
    {
        ProductId = 3,
        Name = "ideapad3i Lenovo",
        Description = "Notebook Lenovo i7",
        Price = 5000,
        Status = ProductStatus.Low,
        CreatedTime = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
    };

    var response = await client.AddProductAsync(new AddProductRequest() { Product = product });

    Console.WriteLine(response);
}

static async Task UpdateProduct(ProductGrpcService.ProductGrpcServiceClient client)
{
    var product = new ProductModel()
    {
        ProductId = 3,
        Name = "ideapad3i Lenovo",
        Description = "Notebook Lenovo Core i7",
        Price = 5000,
        Status = ProductStatus.Low,
        CreatedTime = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
    };

    var response = await client.UpdateProductAsync(new UpdateProductRequest() { Product = product });

    Console.WriteLine(response);
}

static async Task DeleteProductById(ProductGrpcService.ProductGrpcServiceClient client)
{
    var response = await client.DeleteProductAsync(new DeleteProductRequest() { ProductId = 2 });

    Console.WriteLine($"Success: {response.Success}");
}

static async Task InsertBulkProduct(ProductGrpcService.ProductGrpcServiceClient client)
{
    using var clientBulk = client.InsertBulkProduct();

    for (int i = 0; i < 3; i++)
    {
        var product = new ProductModel()
        {
            Name = $"Product {i}",
            Description = $"Bulk insert product {i}",
            Price = 700,
            Status = ProductStatus.InStock,
            CreatedTime = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
        };

        await clientBulk.RequestStream.WriteAsync(product);
    }   

    await clientBulk.RequestStream.CompleteAsync();

    Console.WriteLine("Finish Bulk Insert");
}



