using System.Collections.Generic;
using System.Data.SqlClient;

using Dapper;

namespace Capstone.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<ProductDTO> GetProducts();
    }

    public class ProductRepository : IProductRepository
    {
        private readonly string ConnectionString = @"Server=localhost;Database=Capstone;Trusted_Connection=True";

        public IEnumerable<ProductDTO> GetProducts()
        {
            string query = "SELECT * FROM tblProducts";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var result = conn.Query<ProductDTO>(query);
                return result ?? new List<ProductDTO>();
            }
        }
    }

    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
