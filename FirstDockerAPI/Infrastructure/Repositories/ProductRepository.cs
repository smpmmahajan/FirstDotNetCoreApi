using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FirstDockerAPI.Core.Entities;
using Microsoft.Extensions.Configuration;

namespace FirstDockerAPI.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var conn = this.GetConnection();

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            if (conn.State == ConnectionState.Open)
            {
                string query = "select * from product";
                var result = await SqlMapper.QueryAsync<Product>(conn, query);
                return result;
            }
            return new List<Product>();
        }
        public IDbConnection GetConnection()
        {
            var connectionString = _configuration.GetSection("ConnectionStrings").GetSection("SQLConnection").Value;
            var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            return conn;
        }
    }
}
