using Core.Entities;
using SearchExplorer.Core.Models;

namespace SearchExplorer.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductSearchResponse> SearchProductsAsync(ProductSearchQuery query);
    }
}
