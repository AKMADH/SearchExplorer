using Core.Entities;
using MediatR;
using SearchExplorer.Core.Interfaces;
using SearchExplorer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchExplorer.Application.Handlers
{
    public class ProductSearchQueryHandler : IRequestHandler<ProductSearchQuery, ProductSearchResponse>
    {
        private readonly IProductRepository _productRepository;

        public ProductSearchQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductSearchResponse> Handle(ProductSearchQuery request, CancellationToken cancellationToken)
        {
            // Use the repository to get products
            return await _productRepository.SearchProductsAsync(request);
        }
    }
}
