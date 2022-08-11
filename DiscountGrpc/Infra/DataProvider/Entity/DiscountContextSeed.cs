using DiscountGrpc.Domain;

namespace DiscountGrpc.Infra.DataProvider.Entity
{
    public static class DiscountContextSeed
    {
        public static async Task SeedAsync(DiscountContext context)
        {
            if (!context.Discounts.Any())
            {
                var discounts = new List<Discount>
                {
                    new Discount(1, "CODE_050_pqrstuv", 50),
                    new Discount(2, "CODE_100_liytrgm", 100),
                    new Discount(3, "CODE_200_nbgrtyl", 200),
                    new Discount(4, "CODE_300_rfvdbns", 300)
                };

                await context.Discounts.AddRangeAsync(discounts);
                await context.SaveChangesAsync();   
            }
        }
    }
}
