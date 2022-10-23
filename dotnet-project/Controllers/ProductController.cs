using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using dotnet_project.Models;
using dotnet_project.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace dotnet_project.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productRepo;

        public ProductController(ILogger<ProductController> logger, IProductRepository productRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productRepo.GetProducts();
                return View(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("AddProduct")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost("AddProduct")]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Products product)
        {
            var addedProduct = _productRepo.AddProduct(product);
            return RedirectToAction("Index");
        }
    }
}