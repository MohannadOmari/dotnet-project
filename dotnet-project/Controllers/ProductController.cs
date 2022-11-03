using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Dapper;
using dotnet_project.Models;
using dotnet_project.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    [SecurityCheckAttribute]
    public IActionResult AddProduct()
    {
        ViewBag.userId = HttpContext.Session.GetInt32("UserId");
        ViewBag.userType = HttpContext.Session.GetInt32("UserType");
        return View();
    }

    [HttpPost("AddProduct")]
    [ValidateAntiForgeryToken]
    [SecurityCheckAttribute]
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
        catch (Exception ex)
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
            if (product == null) { return NotFound(); }
            return View(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{Id}/UpdateProduct")]
    [SecurityCheckAttribute]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        ViewBag.userId = userId;
        ViewBag.userType = HttpContext.Session.GetInt32("UserType");

        bool productExists = await _productRepo.GetProductByUserId(id, (int)userId);
        if (productExists)
        {
            return View();
        }
        else
        {
            TempData["error"] = "Unauthorized access! this is not your product";
            return RedirectToAction("Index");
        }
    }

    [HttpPost("UpdateProduct")]
    [SecurityCheckAttribute]
    public async Task<IActionResult> UpdateProduct(Products product, int id)
    {
        if (ModelState.IsValid)
        {
            await _productRepo.UpdateProduct(product, id);
            TempData["success"] = "Product Updated Successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return View(product);
        }
    }

    #region Method SecurityCheckAttribute
    /*
    Checking if user is logged in and a seller
    */
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]

    public class SecurityCheckAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as ProductController;
            var httpContext = controller.HttpContext;

            if (httpContext.Session.GetInt32("UserType") != null)
            {
                var userType = httpContext.Session.GetInt32("UserType");
                if (userType != 2)
                {
                    controller.TempData["error"] = "Unauthorized Access! please login as seller";
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "Controller", "Home" },
                            { "Action", "Index" },
                        });
                    base.OnActionExecuting(context);
                }
            }
            else
            {
                controller.TempData["error"] = "Unauthorized! Access please login";
                context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "Controller", "User" },
                            { "Action", "Login" },
                        });
                base.OnActionExecuting(context);
            }
        }
    }
    #endregion

}