using System.ComponentModel.DataAnnotations;

namespace SearchExplorer.Core.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }  // Price should be decimal for accuracy in financial calculations
        public int StockQuantity { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public decimal Rating { get; set; }  // Changed to 'decimal' from 'float'
    }


    public class ProductSearchResponse
    {
        public List<Product> Products { get; set; }
        public int TotalCount { get; set; }
        public ProductSearchResponse(List<Product> products, int totalCount)
        {
            Products = products;
            TotalCount = totalCount;
        }

    }

}
