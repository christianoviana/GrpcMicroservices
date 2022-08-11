using ProductGrpc.Domain.Enums;
using ProductGrpc.Domain.Models;

namespace ProductGrpc.Infra.DataProvider.Entity
{
    public class ProductContextSeed
    {
        public static async Task SeedAsync(ProductContext context)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>()
                {
                    new Product(1, "Iphone 13", "New iphone 13", 5300, ProductStatus.InStock, DateTime.Now),
                    new Product(2, "Smart TV 75", "Sansumg Smart Tv Curve", 27045, ProductStatus.InStock, DateTime.Now)
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
