using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SearchExplorer.Api.Controllers;
using SearchExplorer.Core.Entities;
using SearchExplorer.Core.Models;
using SearchExplorer.Core.Enums;
using Core.Entities;

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
        public async Task Search_ValidQuery_ReturnsProducts()
        {
            // Arrange
            var query = new ProductSearchQuery { Keyword = "Laptop" };
            var mockProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "Laptop", Price = 1000, Stock = 10, Description = "Gaming Laptop", Category = "Electronics" },
                new Product { ProductId = 2, Name = "MacBook", Price = 2000, Stock = 5, Description = "Apple MacBook", Category = "Electronics" }
            };
            var searchResponse = new ProductSearchResponse(mockProducts, mockProducts.Count);

            _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(searchResponse);

            // Act
            var result = await _controller.Search(query) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var response = result.Value as ApiResponse<ProductSearchResponse>;
            Assert.NotNull(response);
            Assert.AreEqual(StatusCode.OK, response.StatusCode);
            Assert.AreEqual("Product details fetched successfully.", response.Message);
            Assert.AreEqual(2, response.Data.TotalCount);
        }

        [Test]
        public async Task Search_NoProductsFound_ReturnsNotFound()
        {
            // Arrange
            var query = new ProductSearchQuery { Keyword = "NonExistentProduct" };
            var searchResponse = new ProductSearchResponse(new List<Product>(), 0);

            _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(searchResponse);

            // Act
            var result = await _controller.Search(query) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);

            var response = result.Value as ApiResponse<ProductSearchResponse>;
            Assert.NotNull(response);
            Assert.AreEqual(StatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("No products found matching the search criteria.", response.Message);
            Assert.AreEqual(0, response.Data.TotalCount);
        }
    }
}
