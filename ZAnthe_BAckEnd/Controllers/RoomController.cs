using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Models;
using ZAnthe_BAckEnd.ViewModel.Pages;

namespace ZAnthe_BAckEnd.Controllers
{
    public class RoomController : Controller
    {
        private readonly AppDbContext _context;

        public RoomController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Room> room = await _context.rooms.Where(m => !m.isDeleted).ToListAsync();

            RoomVM roomVM = new RoomVM()
            {
                Rooms = room,
            };
                
            return View(roomVM);
        }
    }
}
