using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Capstone.Application.CQRS.Queries;
using Capstone.Infrastructure.Repositories;
using Moq;
using NUnit.Framework;

namespace Capstone.Tests
{
    [TestFixture]
    public class GetProductsQueryHandlerTests
    {
        private Mock<IProductRepository> _mockRepo;
        private GetProductsQueryHandle _handler;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IProductRepository>();
            _handler = new GetProductsQueryHandle(_mockRepo.Object);
        }

        [Test]
        public void Handle_ReturnsAllProducts_WhenNoSearch()
        {
            //Arrange
            var fakeProducts = new List<ProductDTO>
            {
                new ProductDTO { Id = 1, Name = "Candy", UnitPrice = 5 },
                new ProductDTO { Id = 2, Name = "Dark Chocolate", UnitPrice = 10 }
            };

            _mockRepo.Setup(r => r.GetProducts("", "Name", "asc")).Returns(fakeProducts);

            var query = new GetProductsQuery
            {
                Search = "",
                SortBy = "Name",
                SortDirection = "asc"
            };

            //Act
            var result = _handler.Handle(query,CancellationToken.None).Result;

            //Assert
            Assert.AreEqual(2, result.Count);
        }


	}
}
