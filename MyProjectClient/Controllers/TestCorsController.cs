using Microsoft.AspNetCore.Mvc;

namespace MyProjectClient.Controllers
{
    public class TestCorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
