using System.Threading;
using System.Threading.Tasks;
using Capstone.Application.CQRS.Queries;
using Capstone.Core.DTOs;
using Capstone.Core.Interfaces;
using Capstone.Infrastructure.Repositories;
using Moq;
using NUnit.Framework;

namespace Capstone.Tests.CQRS.Queries
{
	[TestFixture]
	public class GetProductByIdQueryHandlerTests
	{
		private Mock<IProductRepository> _mockProductRepository;
		private GetProductByIdQueryHandler _handler;

		[SetUp]
		public void SetUp()
		{
			_mockProductRepository = new Mock<IProductRepository>();
			_handler = new GetProductByIdQueryHandler(_mockProductRepository.Object);
		}

		[Test]
		public async Task Handle_ValidId_ReturnsCorrectProduct()
		{
			// Arrange
			var expectedProduct = new ProductDTO
			{
				Id = 1,
				Name = "Test Product",
				UnitPrice = 99.99m,
				Description = "Test Description",
				ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/56/Candy-Corn.jpg",
				Stock = 10
			};

			var query = new GetProductByIdQuery { Id = 1 };

			_mockProductRepository
				.Setup(r => r.GetProductById(1))
				.ReturnsAsync(expectedProduct);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(expectedProduct.Id, result.Id);
			Assert.AreEqual(expectedProduct.Name, result.Name);
			Assert.AreEqual(expectedProduct.UnitPrice, result.UnitPrice);
			Assert.AreEqual(expectedProduct.Description, result.Description);
			Assert.AreEqual(expectedProduct.ImageUrl, result.ImageUrl);
			Assert.AreEqual(expectedProduct.Stock, result.Stock);
		}

		[Test]
		public async Task Handle_NonExistentId_ReturnsNull()
		{
			// Arrange
			var query = new GetProductByIdQuery { Id = 999 };

			_mockProductRepository
				.Setup(r => r.GetProductById(999))
				.ReturnsAsync((ProductDTO)null);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.IsNull(result);
		}

		[Test]
		public async Task Handle_OutOfStockProduct_ReturnsProductWithZeroStock()
		{
			// Arrange
			var outOfStockProduct = new ProductDTO
			{
				Id = 2,
				Name = "Out of Stock Product",
				UnitPrice = 49.99m,
				Description = "This product is out of stock",
				ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/56/Candy-Corn.jpg",
				Stock = 0
			};

			var query = new GetProductByIdQuery { Id = 2 };

			_mockProductRepository
				.Setup(r => r.GetProductById(2))
				.ReturnsAsync(outOfStockProduct);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Stock);
			Assert.AreEqual(outOfStockProduct.Id, result.Id);
		}

		[Test]
		public async Task Handle_AnyRequest_CallsRepositoryExactlyOnce()
		{
			// Arrange
			var query = new GetProductByIdQuery { Id = 1 };

			_mockProductRepository
				.Setup(r => r.GetProductById(It.IsAny<int>()))
				.ReturnsAsync((ProductDTO)null);

			// Act
			await _handler.Handle(query, CancellationToken.None);

			// Assert
			_mockProductRepository.Verify(
				r => r.GetProductById(It.IsAny<int>()),
				Times.Once,
				"GetProductById should be called exactly once per request"
			);
		}
	}
}
