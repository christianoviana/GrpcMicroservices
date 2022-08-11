using ProductGrpc.Domain.Enums;

namespace ProductGrpc.Domain.Models
{
    public class Product
    {
        public Product()
        {

        }

        public Product(int id, string name, string description, float price, ProductStatus status, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Status = status;
            CreatedAt = createdAt;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public ProductStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsLowStock()
        {
            return Status == ProductStatus.Low;
        }
    }
}
