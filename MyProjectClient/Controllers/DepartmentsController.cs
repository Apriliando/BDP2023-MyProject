using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectClient.Utilities;

namespace MyProjectClient.Controllers
{
    //[Authorize]
    public class DepartmentsController : Controller
    {
        //[Authentication] -> replace with Auhthorize
        public IActionResult Index()
        {
            //var email = TempData["email"];
            //Console.WriteLine("Account / Email, Department: {0}", email);
            //if (email == null)
            //    return RedirectToAction("Login", "Home");
            //else
                return View();
        }
        public IActionResult Charts()
        {
            return View();
        }
    }
}
