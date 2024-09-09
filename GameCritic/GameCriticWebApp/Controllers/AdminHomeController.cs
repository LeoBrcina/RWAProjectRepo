using Microsoft.AspNetCore.Mvc;

namespace GameCriticWebApp.Controllers
{
    public class AdminHomeController : Controller
    {
        private readonly ILogger<AdminHomeController> _logger;
        public AdminHomeController(ILogger<AdminHomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
