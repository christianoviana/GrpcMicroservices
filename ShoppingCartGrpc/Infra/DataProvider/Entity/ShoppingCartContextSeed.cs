using ShoppingCartGrpc.Domain.Models;

namespace ShoppingCartGrpc.Infra.DataProvider.Entity
{
    public static class ShoppingCartContextSeed
    {
        public static async Task SeedAsync(ShoppingCartContext context)
        {
            if (!context.CartItems.Any() && !context.ShoppingCarts.Any())
            {   
                var shoppingCart = new ShoppingCart(null, DateTime.Now.ToString(), "Chris");            

                await context.ShoppingCarts.AddAsync(shoppingCart);

                var cartItem1 = new CartItem(shoppingCart.Id, 8, "Product 1", 250, 1);
                var cartItem2 = new CartItem(shoppingCart.Id, 9, "Product 2", 50, 3);

                shoppingCart.AddItem(cartItem1);
                shoppingCart.AddItem(cartItem2);

                await context.SaveChangesAsync();
            }
        }
    }
}
