using Moq;
using NUnit.Framework;
using SearchExplorer.Api.Controllers;
using SearchExplorer.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SearchExplorer.Core.Models;
using SearchExplorer.Core.Entities;
using SearchExplorer.Core.Enums;

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
            // Create a mock for the IMediator
            _mediatorMock = new Mock<IMediator>();

            // Create an instance of the controller with the mocked mediator
            _controller = new ProductController(_mediatorMock.Object);
        }

        [Test]
        public async Task Search_ReturnsOk_WhenProductsFound()
        {
            // Arrange
            var query = "Product1";
            var filter = ProductFilterEnum.Brand;
            var filterValue = "BrandA";
            var sort = ProductSortEnum.Rating;

            var productSearchQuery = new ProductSearchQuery(query, filter, filterValue, sort);
            var expectedResponse = new ProductSearchResponse(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1", Price = 10.00m, StockQuantity = 100, Rating = 4.5m },
                new Product { ProductId = 2, Name = "Product2", Price = 20.00m, StockQuantity = 150, Rating = 4.0m }
            }, 2);

            // Mock the mediator to return a response
            _mediatorMock.Setup(m => m.Send(It.IsAny<ProductSearchQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Search(query, filter, filterValue, sort);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var apiResponse = okResult.Value as ApiResponse<ProductSearchResponse>;
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(StatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual("Product details fetched successfully.", apiResponse.Message);
            Assert.AreEqual(2, apiResponse.Data.TotalCount);
        }

        [Test]
        public async Task Search_ReturnsNotFound_WhenNoProductsFound()
        {
            // Arrange
            var query = "NonExistingProduct";
            var filter = (ProductFilterEnum?)null;
            var filterValue = (string?)null;
            var sort = (ProductSortEnum?)null;

            var productSearchQuery = new ProductSearchQuery(query, filter, filterValue, sort);
            var expectedResponse = new ProductSearchResponse(new List<Product>(), 0);

            // Mock the mediator to return an empty response
            _mediatorMock.Setup(m => m.Send(It.IsAny<ProductSearchQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Search(query, filter, filterValue, sort);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var apiResponse = notFoundResult.Value as ApiResponse<ProductSearchResponse>;
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(StatusCode.NotFound, apiResponse.StatusCode);
            Assert.AreEqual("No products found matching the search criteria.", apiResponse.Message);
        }

        [Test]
        public async Task Search_ReturnsOk_WhenProductsFilteredByPrice()
        {
            // Arrange
            var query = "Product";
            var filter = ProductFilterEnum.Price;
            var filterValue = "10.00"; // Filter price equals 10
            var sort = ProductSortEnum.Price;

            var productSearchQuery = new ProductSearchQuery(query, filter, filterValue, sort);
            var expectedResponse = new ProductSearchResponse(new List<Product>
            {
                new Product { ProductId = 1, Name = "Product1", Price = 10.00m, StockQuantity = 100, Rating = 4.5m }
            }, 1);

            // Mock the mediator to return a response
            _mediatorMock.Setup(m => m.Send(It.IsAny<ProductSearchQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Search(query, filter, filterValue, sort);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var apiResponse = okResult.Value as ApiResponse<ProductSearchResponse>;
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(StatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual("Product details fetched successfully.", apiResponse.Message);
            Assert.AreEqual(1, apiResponse.Data.TotalCount);
            Assert.AreEqual(10.00m, apiResponse.Data.Products[0].Price); // Check price filter
        }

        [Test]
        public async Task Search_ReturnsBadRequest_WhenQueryIsNullOrEmpty()
        {
            // Arrange
            string query = "";  // Simulating invalid input (empty query)
            var filter = ProductFilterEnum.Brand;
            var filterValue = "BrandA";
            var sort = ProductSortEnum.Price;

            // Act
            var result = await _controller.Search(query, filter, filterValue, sort);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var apiResponse = badRequestResult.Value as ApiResponse<ProductSearchResponse>;
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(StatusCode.BadRequest, apiResponse.StatusCode);
            Assert.AreEqual("Invalid search query.", apiResponse.Message);
        }


        [Test]
        public async Task Search_ReturnsOk_WithProductsSortedByRating()
        {
            // Arrange
            var query = "Product";
            var filter = (ProductFilterEnum?)null;
            var filterValue = (string?)null;
            var sort = ProductSortEnum.Rating;

            // Mocked products to be returned sorted by rating in descending order
            var expectedResponse = new ProductSearchResponse(new List<Product>
                {
                    new Product { ProductId = 1, Name = "Product1", Price = 10.00m, StockQuantity = 100, Rating = 4.5m },
                    new Product { ProductId = 2, Name = "Product2", Price = 20.00m, StockQuantity = 150, Rating = 5.0m }
                }, 2);

            // Mock the mediator to return the expected response
            _mediatorMock.Setup(m => m.Send(It.IsAny<ProductSearchQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Search(query, filter, filterValue, sort);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var apiResponse = okResult.Value as ApiResponse<ProductSearchResponse>;
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(StatusCode.OK, apiResponse.StatusCode);
            Assert.AreEqual(2, apiResponse.Data.TotalCount);
        }

    }
}
