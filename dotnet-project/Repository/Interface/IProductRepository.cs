using dotnet_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_project.Repository.Interface
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Products>> GetProducts();
        public Task AddProduct(Products product);
        public Task<Products> GetProductById(int id);
        public Task UpdateProduct(Products product, int Id);
        public Task<bool> GetProductByUserId(int productId, int userId);
    }
}
