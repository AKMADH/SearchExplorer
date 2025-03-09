using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SearchExplorer.Core.Entities;
using SearchExplorer.Core.Models;
using SearchExplorer.Core.Interfaces;
using Core.Entities;
using SearchExplorer.Application.Services;

namespace SearchExplorer.Tests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Test]
        public async Task SearchProductsAsync_ValidQuery_ReturnsProducts()
        {
            // Arrange
            var query = new ProductSearchQuery { Keyword = "Laptop" };
            var mockProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "Laptop", Price = 1000, Stock = 10, Description = "Gaming Laptop", Category = "Electronics" },
                new Product { ProductId = 2, Name = "MacBook", Price = 2000, Stock = 5, Description = "Apple MacBook", Category = "Electronics" }
            };
            var expectedResponse = new ProductSearchResponse(mockProducts, mockProducts.Count);

            _productRepositoryMock.Setup(repo => repo.SearchProductsAsync(query))
                                  .ReturnsAsync(expectedResponse);

            // Act
            var result = await _productService.SearchProductsAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResponse.TotalCount, result.TotalCount);
            Assert.AreEqual(expectedResponse.Products.Count, result.Products.Count);
            _productRepositoryMock.Verify(repo => repo.SearchProductsAsync(query), Times.Once);
        }
    }
}
