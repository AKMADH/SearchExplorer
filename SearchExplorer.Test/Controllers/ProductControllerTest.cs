using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SearchExplorer.Api.Controllers;
using SearchExplorer.Application.Queries;
using SearchExplorer.Core.Enums;
using SearchExplorer.Core.Models;
using SearchExplorer.core.Entities;
using SearchExplorer.Infrastructure;
using SearchExplorer.Application.Services;
using SearchExplorer.Core.Interfaces;
using SearchExplorer.Core.Entities;
using SearchExplorer.Infrastructure.Repositories;
using SearchExplorer.Core;


namespace SearchExplorer.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductController(_mediatorMock.Object);
        }

        [Test]
        public async Task Search_ReturnsOk_WhenProductsFound()
        {
            // Arrange
            var query = "laptop";
            var filter = ProductFilterEnum.Category;
            var filterValue = "electronics";
            var sort = ProductSortEnum.PriceAsc;
            var response = new ProductSearchResponse { TotalCount = 1 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ProductSearchQuery>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Search(query, filter, filterValue, sort);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<ApiResponse<ProductSearchResponse>>(okResult.Value);
            var apiResponse = okResult.Value as ApiResponse<ProductSearchResponse>;
            Assert.AreEqual(StatusCode.OK, apiResponse.StatusCode);
        }

        [Test]
        public async Task Search_ReturnsNotFound_WhenNoProductsFound()
        {
            // Arrange
            var query = "random item";
            var response = new ProductSearchResponse { TotalCount = 0 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ProductSearchQuery>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Search(query, null, null, null);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.IsInstanceOf<ApiResponse<ProductSearchResponse>>(notFoundResult.Value);
            var apiResponse = notFoundResult.Value as ApiResponse<ProductSearchResponse>;
            Assert.AreEqual(StatusCode.NotFound, apiResponse.StatusCode);
        }
    }
}
