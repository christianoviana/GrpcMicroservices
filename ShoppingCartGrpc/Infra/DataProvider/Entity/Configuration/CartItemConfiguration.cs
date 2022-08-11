using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCartGrpc.Domain.Models;

namespace ShoppingCartGrpc.Infra.DataProvider.Entity.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public CartItemConfiguration()
        {           
        }

        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItem");
            builder.HasKey(e => new { e.ProductId, e.ShoppingCartId });
            builder.Property(e => e.ProductId).HasColumnName("ProductId")
                                              .ValueGeneratedNever();
            builder.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartId")
                                              .ValueGeneratedNever();
            builder.Property(e => e.ProductName).HasColumnName("ProductName").IsRequired();
            builder.Property(e => e.Quantity).HasColumnName("Quantity").IsRequired();
            builder.Property(e => e.Price).HasColumnName("Price").IsRequired().IsRequired();
        }
    }
}
