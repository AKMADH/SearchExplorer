using SearchExplorer.Core.Entities;
using SearchExplorer.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchExplorer.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Laptop", Brand = "Apple", Category = "Electronics", Price = 1000m, Rating = 4.5m, StockQuantity = 10, Description = "High performance laptop" },
                 new Product { ProductId = 2, Name = "Smartphone", Brand = "Iwatch", Category = "Electronics", Price = 700m, Rating = 4.2m, StockQuantity = 15, Description = "Latest model smartphone" },
                 new Product { ProductId = 3, Name = "Shoes", Brand = "Addidas", Category = "Footwear", Price = 50m, Rating = 3.8m, StockQuantity = 20, Description = "Comfortable running shoes" },
                 new Product { ProductId = 4, Name = "Washing Machine", Brand = "Philips", Category = "Appliances", Price = 450m, Rating = 4.0m, StockQuantity = 5, Description = "Energy-efficient washing machine" },
                 new Product { ProductId = 5, Name = "Headphones", Brand = "Sony", Category = "Electronics", Price = 120m, Rating = 4.7m, StockQuantity = 30, Description = "Noise-canceling headphones" }
            };
        }


        // Get all products (In-memory data fetch)
        public Task<List<Product>> GetAllProductsAsync()
        {
            return Task.FromResult(_products);
        }
    }
}
