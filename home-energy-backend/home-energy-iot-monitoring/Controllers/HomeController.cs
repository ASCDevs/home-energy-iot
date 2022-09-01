using Microsoft.AspNetCore.Mvc;

namespace home_energy_iot_monitoring.Controllers
{
    public class HomeController : Controller
    {
        private string _urlSocketDevice { get; set; }
        public HomeController(IConfiguration configuration)
        {
            _urlSocketDevice = configuration["Environment"] == "prod"? configuration["SocketDeviceProd"] : configuration["SocketDeviceDev"];
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Device()
        {
            ViewBag.UrlSocketDevice = _urlSocketDevice;
            return View();
        }
    }
}