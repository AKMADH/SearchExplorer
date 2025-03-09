using Core.Entities;
using Microsoft.EntityFrameworkCore;
using SearchExplorer.Core.Interfaces;
using SearchExplorer.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchExplorer.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) => _context = context;

        public async Task<ProductSearchResponse> SearchProductsAsync(ProductSearchQuery query)
        {
            var products = _context.Products.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(query.Keyword))
                products = products.Where(p => p.Name.Contains(query.Keyword) || p.Description.Contains(query.Keyword));

            if (query.MinPrice.HasValue)
                products = products.Where(p => p.Price >= query.MinPrice.Value);

            if (query.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= query.MaxPrice.Value);

            // Sorting
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy.Equals("Price", System.StringComparison.OrdinalIgnoreCase))
                {
                    products = query.SortDirection == "desc" ? products.OrderByDescending(p => p.Price) : products.OrderBy(p => p.Price);
                }
                else if (query.SortBy.Equals("Name", System.StringComparison.OrdinalIgnoreCase))
                {
                    products = query.SortDirection == "desc" ? products.OrderByDescending(p => p.Name) : products.OrderBy(p => p.Name);
                }
            }

            // Total Count Before Pagination
            int totalCount = await products.CountAsync();
            var productList = await products.ToListAsync();
            // Return response
            return new ProductSearchResponse(productList, totalCount);
        }
    }
}



//using Core.Entities;
//using Microsoft.EntityFrameworkCore;
//using SearchExplorer.Core.Interfaces;
//using SearchExplorer.Core.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SearchExplorer.Infrastructure.Data
//{
//    public class ProductRepository : IProductRepository
//    {
//        private readonly List<Product> _products;

//        // Simulate some in-memory products
//        public ProductRepository()
//        {
//            _products = new List<Product>
//            {
//                new Product { ProductId = 1, Name = "Laptop", Price = 1000, Stock = 10, Description = "A powerful laptop", Category = "Electronics" },
//                new Product { ProductId = 2, Name = "Phone", Price = 500, Stock = 20, Description = "A smartphone", Category = "Electronics" },
//                new Product { ProductId = 3, Name = "T-shirt", Price = 20, Stock = 100, Description = "Comfortable cotton t-shirt", Category = "Apparel" },
//                new Product { ProductId = 4, Name = "Headphones", Price = 150, Stock = 50, Description = "Noise-canceling headphones", Category = "Electronics" },
//                new Product { ProductId = 5, Name = "Shoes", Price = 60, Stock = 75, Description = "Running shoes", Category = "Apparel" }
//            };
//        }

//        public async Task<ProductSearchResponse> SearchProductsAsync(ProductSearchQuery query)
//        {
//            var products = _products.AsQueryable();

//            if (!string.IsNullOrEmpty(query.Keyword))
//            {
//                // Convert both the product name and description to lower case for case-insensitive comparison
//                var lowerKeyword = query.Keyword.ToLower();

//                products = products.Where(p => p.Name.ToLower().Contains(lowerKeyword) || p.Description.ToLower().Contains(lowerKeyword));
//            }

//            if (query.MinPrice.HasValue)
//                products = products.Where(p => p.Price >= query.MinPrice.Value);

//            if (query.MaxPrice.HasValue)
//                products = products.Where(p => p.Price <= query.MaxPrice.Value);

//            if (!string.IsNullOrEmpty(query.SortBy))
//            {
//                if (query.SortBy == "Price")
//                {
//                    products = query.SortDirection == "desc"
//                        ? products.OrderByDescending(p => p.Price)
//                        : products.OrderBy(p => p.Price);
//                }
//                else if (query.SortBy == "Name")
//                {
//                    products = query.SortDirection == "desc"
//                        ? products.OrderByDescending(p => p.Name)
//                        : products.OrderBy(p => p.Name);
//                }
//            }

//            // Create and return the response with the filtered products and total count
//            var response = new ProductSearchResponse(
//                products.ToList(),       // Passing the filtered products
//                products.Count()         // Passing the total count of products
//            );

//            return response;
//        }






//    }
//}
