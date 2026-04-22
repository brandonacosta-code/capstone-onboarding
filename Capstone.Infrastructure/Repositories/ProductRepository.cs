using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Capstone.Core.DTOs;
using Capstone.Core.Interfaces;
using Dapper;

namespace Capstone.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
		private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

		public async Task<IEnumerable<ProductDTO>> GetProducts(string search, string sortBy, string sortDirection)
		{
			var query = "SELECT * FROM dbo.tblProducts WHERE 1=1";

			if (!string.IsNullOrEmpty(search))
			{
				query += " AND Name LIKE @Search";
			}

			// Avoid SQL injection
			var allowedSorts = new[] { "Name", "UnitPrice" };
			if (!allowedSorts.Contains(sortBy))
			{
				sortBy = "Name";
			}

			sortDirection = sortDirection?.ToLower() == "desc" ? "DESC" : "ASC";

			query += $" ORDER BY {sortBy} {sortDirection}";

			using (var conn = new SqlConnection(_connectionString))
			{
				return await conn.QueryAsync<ProductDTO>(query, new
				{
					Search = $"%{search}%"
				});
			}
		}

		public async Task<ProductDTO> GetProductById(int id)
		{
			var query = "SELECT * FROM dbo.tblProducts WHERE Id = @Id";

			using (var conn = new SqlConnection(_connectionString))
			{
				return await conn.QueryFirstOrDefaultAsync<ProductDTO>(query, new { Id = id });
			}
		}

	}
}
