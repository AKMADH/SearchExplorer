using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchExplorer.Core.Entities;
using SearchExplorer.Core.Models;
using System.Text.Json;

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
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ProductSearchQuery query)
        {
            var products = await _mediator.Send(query);
           var response= new ApiResponse<ProductSearchResponse>(Core.Enums.StatusCode.OK, "Product details fetched successfully.", products);
            if (products.TotalCount==0)
            {
                response.StatusCode = Core.Enums.StatusCode.NotFound; 
                response.Message = "No products found matching the search criteria.";
                return NotFound(response);
            }
            return Ok(response);
        }
    }

}
