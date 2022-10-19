using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using dotnet_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace dotnet_project.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration _configuration;

        public ProductController(ILogger<ProductController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IDbConnection Connection
        {
            get
            {
                //System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]
                return new SqlConnection(_configuration.GetConnectionString("DatabaseConnection"));
            }
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("DapperTest")]
        public IActionResult DapperTest()
        {
            using (IDbConnection dbconnection = Connection)
            {
                string sQuery = @"select * from Products";
                dbconnection.Open();
                var list = dbconnection.Query<Products>(sQuery).ToList();
                return View(list);
            }

        }
    }
}