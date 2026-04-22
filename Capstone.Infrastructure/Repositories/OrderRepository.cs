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

		public async Task<int> CreateOrder(CreateOrderDTO order)
		{
			const decimal TAX_RATE = 0.05m;
			const decimal SHIPPING = 10m;

			decimal subTotal = 0;
			foreach (var item in order.Items)
			{
				subTotal += item.Amount;
			}

			decimal tax = subTotal * TAX_RATE;
			decimal total = subTotal + tax + SHIPPING;

			using (var conn = new SqlConnection(_connectionString))
			{
				await conn.OpenAsync();
				using (var transaction = conn.BeginTransaction()) 
				{
					try
					{
						// Insert Order
						const string insertOrder = @"
                    INSERT INTO tblOrders
                        (Subtotal, Tax, Shipping, Total)
                    Values
                        (@SubTotal, @Tax, @Shipping, @Total);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

						int orderId = await conn.QuerySingleAsync<int>(
							insertOrder,
							new
							{
								SubTotal = subTotal,
								Tax = tax,
								Shipping = SHIPPING,
								Total = total
							},
							transaction
						);

						// Insert order items
						const string insertItem = @"
                    INSERT INTO tblOrderItems
                        (OrderId, ProductId, Qty, UnitPrice, Amount)
                    Values
                        (@OrderId, @ProductId, @Qty, @UnitPrice, @Amount);";

						foreach (var item in order.Items)
						{
							await conn.ExecuteAsync(
								insertItem,
								new
								{
									OrderId = orderId,
									ProductId = item.ProductId,
									Qty = item.Qty,
									UnitPrice = item.UnitPrice,
									Amount = item.Amount
								},
								transaction
							);
						}

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
