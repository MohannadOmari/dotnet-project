using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnet_project.Models;
using dotnet_project.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet_project.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepo;

        public UserController(ILogger<UserController> logger, IUserRepository userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password) //add functionality
        {
            try
            {
                await _userRepo.GetUser(Email, Password);
                return Redirect("/");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Users user) //add functionality
        {
            try
            {
                await _userRepo.AddUser(user);
                return RedirectToAction("Login");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}