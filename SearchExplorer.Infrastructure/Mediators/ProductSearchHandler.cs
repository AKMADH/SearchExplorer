using Core.Entities;
using Core.Interfaces;
using MediatR;
using SearchExplorer.Core.Models;


namespace SearchExplorer.Infrastructure.Mediators
{
    public class ProductSearchHandler : IRequestHandler<ProductSearchQuery,ProductSearchResponse>
    {
        private readonly IProductService _productService;
        public ProductSearchHandler(IProductService productService) => _productService = productService;

        public async Task<ProductSearchResponse> Handle(ProductSearchQuery request, CancellationToken cancellationToken) =>
            await _productService.SearchProductsAsync(request);
    }


}