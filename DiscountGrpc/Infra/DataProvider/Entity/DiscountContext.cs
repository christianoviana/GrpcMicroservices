using DiscountGrpc.Domain;
using Microsoft.EntityFrameworkCore;

namespace DiscountGrpc.Infra.DataProvider.Entity
{
    public class DiscountContext: DbContext
    {
        public DiscountContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Discount> Discounts { get; set; }
    }
}
