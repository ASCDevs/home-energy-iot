using Microsoft.AspNetCore.Mvc;

namespace home_energy_iot_monitoring.Controllers
{
    public class HomeController : Controller
    {
        private string _urlSocketDevice { get; set; }
        private string _urlCheckDevice { get; set; }
        private bool _flApiSaveValue { get; set; }
        public HomeController(IConfiguration configuration)
        {
            _urlSocketDevice = configuration["Environment"] == "prod"? configuration["SocketDeviceProd"] : configuration["SocketDeviceDev"];
            _urlCheckDevice = configuration["Environment"] == "prod" ? configuration["CheckDeviceProd"] : configuration["CheckDeviceDev"];
            _flApiSaveValue = Convert.ToBoolean(configuration["flAPISaveValue"]);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Device()
        {
            ViewBag.UrlSocketDevice = _urlSocketDevice;
            ViewBag.FlApiSaveValue = Convert.ToString(_flApiSaveValue).ToLower();
            ViewBag.UrlCheckDevice = _urlCheckDevice;
            return View();
        }
    }
}