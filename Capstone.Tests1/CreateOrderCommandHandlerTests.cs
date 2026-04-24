using System;
using System.Collections.Generic;
using Capstone.Application.CQRS.Commands;
using Capstone.Application.CQRS.Queries;
using Capstone.Core.DTOs;
using Capstone.Core.Interfaces;
using Capstone.Infrastructure.Repositories;
using Moq;
using NUnit.Framework;

namespace Capstone.Tests
{
	[TestFixture]
	public class CreateOrderCommandHandlerTests
	{
		private Mock<IOrderRepository> _mockRepo;
		private CreateOrderCommandHandler _handler;

		[SetUp]
		public void SetUp()
		{
			_mockRepo = new Mock<IOrderRepository>();
			_handler = new CreateOrderCommandHandler(_mockRepo.Object);
		}

		[Test]
		public void Handle_ValidOrder_ReturnsOrderSummary()
		{
			// Arrange
			_mockRepo.Setup(r => r.CreateOrder(
				It.IsAny<CreateOrderDTO>(),
				It.IsAny<decimal>(),  // subTotal
				It.IsAny<decimal>(),  // tax
				It.IsAny<decimal>(),  // shipping
				It.IsAny<decimal>()   // total
			)).ReturnsAsync(1);

			var order = new CreateOrderDTO
			{
				Items = new List<OrderItemDTO>
				{
					new OrderItemDTO
					{
						ProductId = 1,
						Name      = "Candy",
						Qty       = 2,
						UnitPrice = 5,
						Amount    = 10
					}
				}
			};

			var command = new CreateOrderCommand(order);

			// Act
			var result = _handler.Handle(command, default).Result;

			// Assert
			Assert.AreEqual(1, result.OrderId);
			Assert.AreEqual(10, result.Subtotal);
			Assert.AreEqual(0.5m, result.Tax);
			Assert.AreEqual(10, result.Shipping);
			Assert.AreEqual(20.5m, result.Total);
		}

		[Test]
		public void Handle_MultipleItems_CalculatesTotalsCorrectly()
		{
			// Arrange
			_mockRepo.Setup(r => r.CreateOrder(
				It.IsAny<CreateOrderDTO>(),
				It.IsAny<decimal>(),  // subTotal
				It.IsAny<decimal>(),  // tax
				It.IsAny<decimal>(),  // shipping
				It.IsAny<decimal>()   // total
			)).ReturnsAsync(2);

			var order = new CreateOrderDTO
			{
				Items = new List<OrderItemDTO>
				{
					new OrderItemDTO
					{ ProductId = 1, Qty = 1,
					  UnitPrice = 20, Amount = 20 },
					new OrderItemDTO
					{ ProductId = 2, Qty = 2,
					  UnitPrice = 15, Amount = 30 }
				}
			};

			var command = new CreateOrderCommand(order);

			// Act
			var result = _handler.Handle(command, default).Result;

			// Assert
			Assert.AreEqual(50, result.Subtotal);
			Assert.AreEqual(2.5m, result.Tax);
			Assert.AreEqual(10, result.Shipping);
			Assert.AreEqual(62.5m, result.Total);
		}

		[Test]
		public void Handle_CallsRepository_Once()
		{
			// Arrange
			_mockRepo.Setup(r => r.CreateOrder(
				It.IsAny<CreateOrderDTO>(),
				It.IsAny<decimal>(),  // subTotal
				It.IsAny<decimal>(),  // tax
				It.IsAny<decimal>(),  // shipping
				It.IsAny<decimal>()   // total
			)).ReturnsAsync(3);

			var order = new CreateOrderDTO
			{
				Items = new List<OrderItemDTO>
				{
					new OrderItemDTO
					{ ProductId = 1, Qty = 1,
					  UnitPrice = 10, Amount = 10 }
				}
			};

			// Act
			_handler.Handle(new CreateOrderCommand(order), default).Wait();

			// Assert
			_mockRepo.Verify((r => r.CreateOrder(
				It.IsAny<CreateOrderDTO>(),
				It.IsAny<decimal>(),  // subTotal
				It.IsAny<decimal>(),  // tax
				It.IsAny<decimal>(),  // shipping
				It.IsAny<decimal>()   // total
			)),
				Times.Once
			);
		}

		[Test]
		public void Handle_EmptyItems_ThrowsArgumentException()
		{
			// Arrange
			var order = new CreateOrderDTO
			{
				Items = new List<OrderItemDTO>()
			};
			var command = new CreateOrderCommand(order);

			Assert.Throws<ArgumentException>(() =>
				_handler.Handle(command, default).GetAwaiter().GetResult()
			);
		}

		[Test]
		public void Handle_NullItems_ThrowsArgumentException()
		{
			var order = new CreateOrderDTO { Items = null };
			var command = new CreateOrderCommand(order);

			Assert.Throws<ArgumentException>(() =>
				_handler.Handle(command, default).GetAwaiter().GetResult()
			);
		}

		[Test]
		public void Handle_ItemWithZeroQty_ThrowsArgumentException()
		{
			var order = new CreateOrderDTO
			{
				Items = new List<OrderItemDTO>
		{
			new OrderItemDTO { ProductId = 1, Qty = 0, UnitPrice = 10, Amount = 10 }
		}
			};
			var command = new CreateOrderCommand(order);

			Assert.Throws<ArgumentException>(() =>
				_handler.Handle(command, default).GetAwaiter().GetResult()
			);
		}

		[Test]
		public void Handle_ItemWithNegativeAmount_ThrowsArgumentException()
		{
			var order = new CreateOrderDTO
			{
				Items = new List<OrderItemDTO>
		{
			new OrderItemDTO { ProductId = 1, Qty = 1, UnitPrice = 10, Amount = -10 }
		}
			};
			var command = new CreateOrderCommand(order);

			Assert.Throws<ArgumentException>(() =>
				_handler.Handle(command, default).GetAwaiter().GetResult()
			);
		}

	}
}

