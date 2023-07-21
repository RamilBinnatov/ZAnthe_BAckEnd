using Microsoft.AspNetCore.Mvc;

namespace ZAnthe_BAckEnd.Controllers
{
    public class HotelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
