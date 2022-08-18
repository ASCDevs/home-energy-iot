using Microsoft.AspNetCore.Mvc;

namespace home_energy_iot_monitoring.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Device()
        {
            return View();
        }
    }
}