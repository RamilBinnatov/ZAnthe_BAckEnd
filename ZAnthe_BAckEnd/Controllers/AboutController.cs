using Microsoft.AspNetCore.Mvc;

namespace ZAnthe_BAckEnd.Controllers
{
    public class AboutController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
