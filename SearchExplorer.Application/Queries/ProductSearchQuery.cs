using MediatR;
using SearchExplorer.Core.Entities;

namespace SearchExplorer.Application.Queries
{
    public class ProductSearchQuery : IRequest<ProductSearchResponse>
    {
        public string Query { get; }
        public ProductFilterEnum? Filter { get; } // Optional filter type (e.g., by brand, price, etc.)
        public string? FilterValue { get; }  // The value for the filter (e.g., the brand name or price)
        public ProductSortEnum? Sort { get; } // Optional sort type
        public decimal? MinPrice { get; set; } // Optional Min Price filter
        public decimal? MaxPrice { get; set; } // Optional Max Price filter
        public string? SortDirection { get; set; } // Optional Sort Direction (asc/desc)

        public ProductSearchQuery(string query, ProductFilterEnum? filter = null, string? filterValue = null, ProductSortEnum? sort = null, decimal? minPrice = null, decimal? maxPrice = null, string? sortDirection = null)
        {
            Query = query;
            Filter = filter;
            FilterValue = filterValue;
            Sort = sort;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            SortDirection = sortDirection;
        }
    }
}
