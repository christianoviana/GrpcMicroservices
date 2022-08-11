using Microsoft.EntityFrameworkCore;
using ProductGrpc.Domain.Models;

namespace ProductGrpc.Infra.DataProvider.Entity
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
    }
}
