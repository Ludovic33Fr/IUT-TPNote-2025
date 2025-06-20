using IUT_TPNote_2025.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IUT_TPNote_2025.Controllers
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public PartialViewResult GetListeImpression()
        {
            return PartialView("_ListeEtatImpressionPartial");
        }
    }
}