namespace ShoppingCartGrpc.Domain.Models
{
    public class ShoppingCart
    {
        protected ShoppingCart()
        {
        }

        public ShoppingCart(int id, List<CartItem> items, string data, string userName)
        {
            Id = id;
            Items = items;
            Data = data;
            UserName = userName;
        }

        public ShoppingCart(List<CartItem> items, string data, string userName)
        {
            Data = data;
            Items = items;
            UserName = userName;
        }

        public int Id { get; protected set; }
        public List<CartItem> Items { get; private set; }
        public string Data { get; private set; }
        public string UserName { get; private set; }

        public float TotalAmount()
        {
            if (Items.Any())
                return Items.Sum(i => i.TotalPrice());

            return 0;
        }

        public float TotalQuantity()
        {
            if (Items.Any())
                return Items.Sum(i => i.Quantity);

            return 0;
        }

        public float TotalItems()
        {
            if (Items.Any())
                return Items.Count;

            return 0;
        }

        public void Clear()
        {
            Items?.Clear();
        }

        public void AddItem(CartItem item)
        {
            if (Items == null)
                Items = new List<CartItem>();

            Items.Add(item);
        }
    }
}
