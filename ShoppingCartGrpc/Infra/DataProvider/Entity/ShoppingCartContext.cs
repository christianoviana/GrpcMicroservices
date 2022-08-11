using Microsoft.EntityFrameworkCore;
using ShoppingCartGrpc.Domain.Models;
using ShoppingCartGrpc.Infra.DataProvider.Entity.Configuration;

namespace ShoppingCartGrpc.Infra.DataProvider.Entity
{
    public class ShoppingCartContext : DbContext
    {
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public ShoppingCartContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
