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

namespace dotnet_project.Controllers;

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
            ViewBag.userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userType = HttpContext.Session.GetInt32("UserType");
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
    public async Task<IActionResult> AddProduct(Products product)
    {
        try
        {
            if (ModelState.IsValid)
            {
                product.SellerId = (int)HttpContext.Session.GetInt32("UserId");
                await _productRepo.AddProduct(product);
                TempData["success"] = "Product added succesfully";
                return RedirectToAction("Index");
            }
            return View(product);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            ViewBag.userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userType = HttpContext.Session.GetInt32("UserType");
            var product = await _productRepo.GetProductById(id);
            if (product == null){return NotFound();}
            return View(product);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
