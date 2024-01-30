using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Session.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection fc)
        {
            string res = fc["txname"];
            return View();
        }

    }
}