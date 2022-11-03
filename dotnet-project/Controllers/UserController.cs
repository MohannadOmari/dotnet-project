using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnet_project.Models;
using dotnet_project.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

        #region Login
        [HttpGet("/Login")]
        public IActionResult Login()
        {
            ViewBag.userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userType = HttpContext.Session.GetInt32("UserType");
            return View();
        }

        [HttpPost("/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Users user) //add functionality
        {
            try
            {
                Users? loggedIn = await _userRepo.GetUser(user);
                if (loggedIn != null)
                {
                    await HttpContext.Session.LoadAsync();
                    HttpContext.Session.SetInt32("UserId", loggedIn.Id);
                    HttpContext.Session.SetInt32("UserType", loggedIn.UserTypeId);
                    await HttpContext.Session.CommitAsync();
                    TempData["success"] = "Logged in succesfully";
                    return Redirect("/");
                }
                else
                {
                    TempData["error"] = "Login information incorrect";
                    return View();
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Register
        [HttpGet("/Register")]
        public IActionResult Register()
        {
            ViewBag.userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userType = HttpContext.Session.GetInt32("UserType");
            return View();
        }

        [HttpPost("/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Users user) //add functionality
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _userRepo.AddUser(user);
                    return RedirectToAction("Login");
                }
                return View(user);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Profile
        [HttpGet("/Profile")]
        [SecurityCheckAttribute]
        public async Task<IActionResult> Profile()
        {
            ViewBag.userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userType = HttpContext.Session.GetInt32("UserType");
            int? id = HttpContext.Session.GetInt32("UserId");
            Users user = await _userRepo.GetUserById((int)id);
            return View(user);
        }

        [HttpGet("Profile/Edit")]
        [SecurityCheckAttribute]
        public IActionResult EditProfile()
        {
            ViewBag.userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userType = HttpContext.Session.GetInt32("UserType");
            return View();
        }

        [HttpPost("Profile/Edit")]
        [SecurityCheckAttribute]
        public async Task<IActionResult> EditProfile(Users user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            int? id = HttpContext.Session.GetInt32("UserId");
            await _userRepo.UpdateUser(user, id);
            TempData["success"] = "Profile updated successfully";
            return RedirectToAction("Profile");
        }
        #endregion

        [HttpGet("/Logout")]
        [SecurityCheckAttribute]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        #region Method SecurityCheckAttribute
        /*
        Security check for authorization
        if user not logged in route them to logging in page
        */
        [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]

        public class SecurityCheckAttribute : ActionFilterAttribute, IActionFilter
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var controller = context.Controller as UserController;
                var httpContext = controller.HttpContext;

                if (httpContext.Session.GetInt32("UserId") == null)
                {
                    
                    controller.TempData["error"] = "Unauthorized Access please login";
                    context.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                            { "Controller", "User" },
                            { "Action", "Login" },
                            });
                    base.OnActionExecuting(context);
                }

                base.OnActionExecuting(context);
            }
        }
        #endregion
    }
}