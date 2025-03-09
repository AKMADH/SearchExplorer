using Core.Entities;
using SearchExplorer.Core.Models;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductSearchResponse> SearchProductsAsync(ProductSearchQuery query);
    }
}
