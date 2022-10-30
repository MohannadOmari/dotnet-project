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

        // used to retrieve all products
        public async Task<IEnumerable<Products>> GetProducts()
        {
            var query = "SELECT * FROM Products";
            using(var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Products>(query);
                return products.ToList();
            }
        }

        // used to get one product using its Id
        public async Task<Products> GetProductById(int id)
        {
            var query = "SELECT * FROM Products WHERE Id = @Id";
            using(var connection = _context.CreateConnection())
            {
                var product = await connection.QuerySingleOrDefaultAsync<Products>(query, new { id });
                return product;
            }
        }

        // used to add products into the database
        public async Task AddProduct(Products product)
        {
            var query = "INSERT INTO Products (ProductName, Quantity, Price, SellerId, CreatedAt, SoldAt, ProductTypeId, ProductDescription)" 
                + "VALUES (@ProductName, @Quantity, @Price, @SellerId, @CreatedAt, @SoldAt, @ProductTypeId, @ProductDescription)";

            var parameters = new DynamicParameters();
            parameters.Add("ProductName", product.ProductName);
            parameters.Add("Quantity", product.Quantity);
            parameters.Add("Price", product.Price);
            parameters.Add("SellerId", product.SellerId);
            parameters.Add("CreatedAt", product.CreatedAt);
            parameters.Add("SoldAt", null);
            parameters.Add("ProductTypeId", product.ProductTypeId);
            parameters.Add("ProductDescription", product.ProductDescription);

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }

        }
    }
}
