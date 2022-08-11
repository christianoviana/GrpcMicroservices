namespace DiscountGrpc.Domain
{
    public class Discount
    {
        public Discount(int id, string code, int amount)
        {
            Id = id;
            Code = code;
            Amount = amount;
        }

        public int Id { get; private set; }
        public string Code { get; private set; }
        public int Amount { get; private set; }
    }
}
