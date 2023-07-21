using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Models;
using ZAnthe_BAckEnd.ViewModel.Pages;

namespace ZAnthe_BAckEnd.Controllers
{
    public class RoomDetailController : Controller
    {
        private readonly AppDbContext _context;
        public RoomDetailController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            Room room = await _context.rooms.Where(m => !m.isDeleted && m.Id == id)
                .Include(m => m.RoomServicePivots)
                .ThenInclude(m=>m.roomService)
                .FirstOrDefaultAsync();
            IEnumerable<Room> rooms = await _context.rooms.Where(m => !m.isDeleted && m.Id != id)
                .Take(3)
                .ToListAsync();
            List<RoomServicePivot> roomServicePivots = await _context.roomServicePivots.Where(m=>m.RoomId == id).ToListAsync();
            List<RoomService> roomServices = new List<RoomService>();
            foreach (var service in roomServicePivots)
            {
                RoomService roomService = await _context.roomServices.Where(m => m.Id == service.RoomServiceId).FirstOrDefaultAsync();
                roomServices.Add(roomService);
            }

            RoomDetailVM roomDetailVM = new RoomDetailVM()
            {
                SimilarRooms = rooms,
                room = room,
                roomServices = roomServices,
            };

            return View(roomDetailVM);
        }
    }
}
