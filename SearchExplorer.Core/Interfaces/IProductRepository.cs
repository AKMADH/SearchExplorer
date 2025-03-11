using SearchExplorer.Core.Entities;
using SearchExplorer.Core.Models;

namespace SearchExplorer.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync(); 
    }
}
