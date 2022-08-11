using System.ComponentModel.DataAnnotations;

namespace ShoppingCartGrpc.Domain.Models
{
    public class CartItem
    {
        protected CartItem()
        {
        }

        public CartItem(int shoppingCartId, int productId, string productName, float price, int quantity)
        {
            ShoppingCartId = shoppingCartId;
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        public int ShoppingCartId { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public float Price { get; private set; }
        public int Quantity { get; private set; }  

        public float TotalPrice()
        {
           return Price * Quantity;
        }

        public float TotalQuantity()
        {
            return Quantity;
        }

        public void SetDiscount(int discount)
        {
            this.Price -= discount;
        }

        public void IncreaseQuantity()
        {
            Quantity = Quantity + 1;
        }
        public void DecreaseQuantity()
        {
            Quantity = Quantity - 1;
        }

        public void SetShoppingCartId(int id)
        {
            this.ShoppingCartId = id;
        }
    }
}
