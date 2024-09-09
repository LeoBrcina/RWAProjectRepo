using GameCriticBL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameCriticWebApp.Controllers
{
    public class AjaxTestController : Controller
    {
        private readonly RwaprojectDbContext _context;

        public ActionResult Index()
        {
            return View();
        }

        public AjaxTestController(RwaprojectDbContext context)
        {
            _context = context;
        }

        public IActionResult AjaxHtml()
        {
            return View();
        }
    }
}
