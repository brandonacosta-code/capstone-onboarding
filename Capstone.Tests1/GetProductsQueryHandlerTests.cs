using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Capstone.Application.CQRS.Queries;
using Capstone.Core.DTOs;
using Capstone.Core.Interfaces;
using Capstone.Infrastructure.Repositories;
using Moq;
using NUnit.Framework;

namespace Capstone.Tests1
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

			_mockRepo.Setup(r => r.GetProducts("", "Name", "asc")).ReturnsAsync(fakeProducts);

			var query = new GetProductsQuery
			{
				Search = "",
				SortBy = "Name",
				SortDirection = "asc"
			};

			//Act
			var result = _handler.Handle(query, CancellationToken.None).Result;

			//Assert
			Assert.AreEqual(2, result.Count);
		}

		[Test]
		public void Handle_ReturnsFilteredProducts_WhenSearchProvided()
		{
			//Arrange
			var fakeProducts = new List<ProductDTO>
			{
				new ProductDTO { Id = 2,Name = "Dark Chocolate", UnitPrice = 10 },
				new ProductDTO { Id = 3,Name = "Milk Chocolate", UnitPrice = 8 }
			};

			_mockRepo.Setup(r => r.GetProducts("Chocolate", "Name", "asc")).ReturnsAsync(fakeProducts);

			var query = new GetProductsQuery
			{
				Search = "Chocolate",
				SortBy = "Name",
				SortDirection = "asc"
			};

			//Act
			var result = _handler.Handle(query, CancellationToken.None).Result;

			//Assert
			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.All(p => p.Name.Contains("Chocolate")));
		}

		[Test]
		public void Handle_ReturnsSortedByPriceAscending()
		{
			//Arrange
			var fakeProducts = new List<ProductDTO>
			{
				new ProductDTO { Id = 3,Name = "Milk Chocolate", UnitPrice = 8 },
				new ProductDTO { Id = 2,Name = "Dark Chocolate", UnitPrice = 10 }
			};

			_mockRepo.Setup(r => r.GetProducts("", "UnitPrice", "asc")).ReturnsAsync(fakeProducts);

			var query = new GetProductsQuery
			{
				Search = "",
				SortBy = "UnitPrice",
				SortDirection = "asc"
			};

			//Act
			var result = _handler.Handle(query, CancellationToken.None).Result;

			//Assert
			Assert.AreEqual(8, result.First().UnitPrice);
		}

		[Test]
		public void Handle_CallsRepository_ExactlyOnce()
		{
			//Arrange
			_mockRepo.Setup(r => r.GetProducts(
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<string>()))
				.ReturnsAsync(new List<ProductDTO>());

			var query = new GetProductsQuery
			{
				Search = "",
				SortBy = "Name",
				SortDirection = "asc"
			};

			//Act
			_handler.Handle(query, CancellationToken.None).Wait();

			//Assert
			_mockRepo.Verify(r => r.GetProducts(
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<string>()),
				Times.Once);
		}
	}
}
