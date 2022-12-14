using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnet_project.Models;

namespace dotnet_project.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.userId = HttpContext.Session.GetInt32("UserId");
        ViewBag.userType = HttpContext.Session.GetInt32("UserType");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
