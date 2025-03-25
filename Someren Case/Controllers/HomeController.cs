using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Someren_Case.Models;
using System.Diagnostics;

namespace Someren_Case.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Error handling method to display request errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Using TraceIdentifier directly for the RequestId
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}