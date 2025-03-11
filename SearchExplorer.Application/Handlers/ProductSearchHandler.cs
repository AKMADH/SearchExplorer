using MediatR;
using SearchExplorer.Core.Interfaces;
using SearchExplorer.Core.Models;
using SearchExplorer.Application.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SearchExplorer.Core.Entities;
using System;

namespace SearchExplorer.Application.Handlers
{
    public class ProductSearchHandler : IRequestHandler<ProductSearchQuery, ProductSearchResponse>
    {
        private readonly IProductRepository _productRepository;

        public ProductSearchHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductSearchResponse> Handle(ProductSearchQuery request, CancellationToken cancellationToken)
        {
            // Get all products from the repository
            var products = await _productRepository.GetAllProductsAsync();

            // Apply query-based filter (search by name)
            if (!string.IsNullOrEmpty(request.Query))
            {
                products = products.Where(p => p.Name.Contains(request.Query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply price range filter (MinPrice and MaxPrice)
            if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
            {
                products = products.Where(p =>
                    (!request.MinPrice.HasValue || p.Price >= request.MinPrice.Value) &&
                    (!request.MaxPrice.HasValue || p.Price <= request.MaxPrice.Value)
                ).ToList();
            }

            // Apply additional filter based on ProductFilterEnum (e.g., by brand, rating, category)
            if (request.Filter.HasValue)
            {
                switch (request.Filter.Value)
                {
                    case ProductFilterEnum.Brand:
                        if (!string.IsNullOrEmpty(request.FilterValue))
                        {
                            products = products.Where(p => p.Brand.Equals(request.FilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                        break;
                    case ProductFilterEnum.Rating:
                        if (decimal.TryParse(request.FilterValue, out decimal rating))
                        {
                            products = products.Where(p => p.Rating >= rating).ToList();
                        }
                        break;
                    case ProductFilterEnum.Category:
                        if (!string.IsNullOrEmpty(request.FilterValue))
                        {
                            products = products.Where(p => p.Category.Equals(request.FilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                        break;
                    case ProductFilterEnum.Price:
                        if (decimal.TryParse(request.FilterValue, out decimal price))
                        {
                            products = products.Where(p => p.Price <= price).ToList();
                        }
                        break;
                }
            }

            // Apply sorting if needed
            if (request.Sort.HasValue)
            {
                products = SortProducts(products, request.Sort.Value, request.SortDirection).ToList();
            }

            // Get the total count of products after filters
            var totalCount = products.Count;

            // Map to response
            return new ProductSearchResponse(products, totalCount);
        }

        private IEnumerable<Product> SortProducts(IEnumerable<Product> products, ProductSortEnum sortTerm, string? sortDirection)
        {
            // Perform sorting based on the specified sort term
            var sortedProducts = sortTerm switch
            {
                ProductSortEnum.Name => products.OrderBy(p => p.Name),
                ProductSortEnum.Price => products.OrderBy(p => p.Price),
                ProductSortEnum.Rating => products.OrderByDescending(p => p.Rating),
                _ => products
            };

            // If SortDirection is Descending, reverse the order
            if (sortDirection != null && sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                sortedProducts = sortedProducts.Reverse();
            }

            return sortedProducts;
        }
    }
}
