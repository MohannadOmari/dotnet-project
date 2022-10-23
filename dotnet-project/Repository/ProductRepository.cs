using Dapper;
using dotnet_project.Context;
using dotnet_project.Models;
using dotnet_project.Repository.Interface;

namespace dotnet_project.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Products>> GetProducts()
        {
            var query = "SELECT * FROM Products";
            using(var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Products>(query);
                return products.ToList();
            }
        }

        public async Task AddProduct(Products product)
        {
            var query = "INSERT INTO Products (ProductName, Quantity, Price, SellerId, CreatedAt, SoldAt, ProductTypeId)" 
                + "VALUES (@ProductName, @Quantity, @Price, @SellerId, @CreatedAt, @SoldAt, @ProductTypeId)";

            var parameters = new DynamicParameters();
            parameters.Add("ProductName", product.ProductName, System.Data.DbType.String);
            parameters.Add("Quantity", product.Quantity, System.Data.DbType.Int32);
            parameters.Add("Price", product.Price, System.Data.DbType.Int32);
            parameters.Add("SellerId", product.SellerId, System.Data.DbType.Int32);
            parameters.Add("CreatedAt", product.CreatedAt, System.Data.DbType.DateTime);
            parameters.Add("SoldAt", null, System.Data.DbType.Int32);
            parameters.Add("ProductTypeId", product.ProductTypeId, System.Data.DbType.Int32);

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }

        }
    }
}
