using Microsoft.AspNetCore.Mvc;
using MediatR;
using SearchExplorer.Application.Queries;
using SearchExplorer.Core.Models;
using SearchExplorer.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace SearchExplorer.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator ;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string query,
            [FromQuery] ProductFilterEnum? filter = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] ProductSortEnum? sort = null)
        {

            if (string.IsNullOrWhiteSpace(query))
            {
                var result = new ApiResponse<ProductSearchResponse>(
                    Core.Enums.StatusCode.BadRequest,
                    "Invalid search query.",
                    null
                );
                return BadRequest(result);
            }
            // Create the query object to pass to the mediator
            var productSearchQuery = new ProductSearchQuery(query, filter, filterValue, sort);

            // Send the query to the handler
            var products = await _mediator.Send(productSearchQuery);

            // Creating response
            var response = new ApiResponse<ProductSearchResponse>(Core.Enums.StatusCode.OK, "Product details fetched successfully.", products);

            // If no products were found, return NotFound
            if (products.TotalCount == 0)
            {
                response.StatusCode = Core.Enums.StatusCode.NotFound;
                response.Message = "No products found matching the search criteria.";
                return NotFound(response);
            }

            // Return the products with the response
            return Ok(response);
        }
    }
}
