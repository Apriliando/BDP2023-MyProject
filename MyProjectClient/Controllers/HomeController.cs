using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectClient.Models;
using MyProjectClient.Utilities;
using System.Diagnostics;
using System.Security.Principal;

namespace MyProjectClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            //if (HttpContext.Session.GetString("email") == null)
            //{
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Departments");
            //}
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Login(string email)
        {
            //if (HttpContext.Session.GetString("email") == null)
            //{
            //    if (ModelState.IsValid) //IMPORTANT DO NOT COMMENT OUT IF USING SESSION INSTEAD OF JWT
            //    {
            //        HttpContext.Session.SetString("email", email.ToString());
                    //TempData["email"] = email;
                    Console.WriteLine("Account / Email, Home: {0}", email);
                    return RedirectToAction("Index", "Departments");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
            //return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("email");

            return RedirectToAction("Home");
        }

        //[Authentication] -> replace with Authorize
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}