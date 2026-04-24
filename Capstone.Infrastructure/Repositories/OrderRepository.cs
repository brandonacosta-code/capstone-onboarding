using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Core.DTOs;
using Capstone.Core.Interfaces;
using Dapper;

namespace Capstone.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
		private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

		public async Task<int> CreateOrder(CreateOrderDTO order, decimal subTotal, decimal tax, decimal shipping, decimal total)
		{
			using (var conn = new SqlConnection(_connectionString))
			{
				await conn.OpenAsync();
				using (var transaction = conn.BeginTransaction())
				{
					try
					{
						const string insertOrder = @"
                    INSERT INTO tblOrders
                        (Subtotal, Tax, Shipping, Total)
                    VALUES
                        (@SubTotal, @Tax, @Shipping, @Total);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

						int orderId = await conn.QuerySingleAsync<int>(
							insertOrder,
							new { SubTotal = subTotal, Tax = tax, Shipping = shipping, Total = total },
							transaction
						);

						const string insertItem = @"
                    INSERT INTO tblOrderItems
                        (OrderId, ProductId, Qty, UnitPrice, Amount)
                    VALUES
                        (@OrderId, @ProductId, @Qty, @UnitPrice, @Amount);";

						await conn.ExecuteAsync(
							insertItem,
							order.Items.Select(item => new
							{
								OrderId = orderId,
								ProductId = item.ProductId,
								Qty = item.Qty,
								UnitPrice = item.UnitPrice,
								Amount = item.Amount
							}),
							transaction
						);

						transaction.Commit();
						return orderId;
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}



	}
}
