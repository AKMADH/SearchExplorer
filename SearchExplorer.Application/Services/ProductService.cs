using Core.Entities;
using Core.Interfaces;
using SearchExplorer.Core.Interfaces;
using SearchExplorer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchExplorer.Application.Services
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository) => _productRepository = productRepository;
        public async Task<ProductSearchResponse> SearchProductsAsync(ProductSearchQuery query) => await _productRepository.SearchProductsAsync(query);
    }
}
