using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
    public class ProductSearchResponse
    {
        // Make sure this property is public so it's accessible
        public List<Product> Products { get; set; }
        public int TotalCount { get; set; }

        // You can also provide a constructor to initialize the properties if needed
        public ProductSearchResponse(List<Product> products, int totalCount)
        {
            Products = products;
            TotalCount = totalCount;
        }

        // If you don't need a constructor, the default one will work as long as the properties are public.
    }

}
