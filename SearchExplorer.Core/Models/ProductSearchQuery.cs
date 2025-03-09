using Core.Entities;
using MediatR;
namespace SearchExplorer.Core.Models
{
    public class ProductSearchQuery : IRequest<ProductSearchResponse>
    {
        public string? Keyword { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }

}
