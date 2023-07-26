using Microsoft.AspNetCore.Mvc;

namespace ZAnthe_BAckEnd.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
