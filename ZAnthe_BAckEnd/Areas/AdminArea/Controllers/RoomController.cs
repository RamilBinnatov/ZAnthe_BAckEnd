using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Helpers.Enums;
using ZAnthe_BAckEnd.Helpers;
using ZAnthe_BAckEnd.Models;
using Microsoft.EntityFrameworkCore;
using ZAnthe_BAckEnd.ViewModel.Room;

namespace ZAnthe_BAckEnd.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class RoomController : Controller
    {
        #region Readonly
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        #endregion

        #region Constructor
        public RoomController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Room> rooms = await _context.rooms
                .Where(m => !m.isDeleted)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            ViewBag.take = take;

            List<RoomListVM> mapDatas = GetMapDatas(rooms);

            int count = await GetPageCount(take);

            Paginate<RoomListVM> result = new Paginate<RoomListVM>(mapDatas, page, count);

            return View(result);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var data = await GetServiceAsync();

            RoomCreateVM roomCreateVM = new RoomCreateVM()
            {
                RoomServices = data
            };


            return View(roomCreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomCreateVM room)
        {

            if (!ModelState.IsValid) return View();

            if (!room.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!room.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + room.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/allimg", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await room.Photo.CopyToAsync(stream);
            }



            Room newRoom = new Room
            {
                Name = room.Name,
                Desc = room.Desc,
                Price= room.Price,
            };

            await _context.rooms.AddAsync(newRoom);

            await _context.SaveChangesAsync();


            foreach (var item in room.RoomServices.Where(m => m.IsSelected))
            {
                RoomServicePivot roomServicePivot = new RoomServicePivot
                {
                    RoomId = newRoom.Id,
                    RoomServiceId = item.Id,
                };
                await _context.roomServicePivots.AddAsync(roomServicePivot);
            }

            
            _context.rooms.Update(newRoom);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Room room = await _context.rooms
                .Where(m => !m.isDeleted && m.Id == id)
                .FirstOrDefaultAsync();

            if (room == null) return NotFound();

            room.isDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        #endregion



        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Room dbRoom = await GetByIdAsync((int)id);

            return View(new RoomEditVM
            {
                Name = dbRoom.Name,
                Desc = dbRoom.Desc,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomEditVM roomEditVM)
        {

            if (!ModelState.IsValid) return View(roomEditVM);

            Room dbRoom = await GetByIdAsync(id);

            if (!ModelState.IsValid) return View();

            if (!roomEditVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!roomEditVM.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + roomEditVM.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/room", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await roomEditVM.Photo.CopyToAsync(stream);
            }


            return View(new RoomEditVM
            {
                Name = dbRoom.Name,
                Desc = dbRoom.Desc,
                Image = fileName
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<RoomService>> GetServiceAsync()
        {
            List<RoomService> service = await _context.roomServices.Where(m => !m.isDeleted).ToListAsync();
            return service;
        }

        #endregion

        #region Services



        private async Task<Room> GetByIdAsync(int id)
        {
            return await _context.rooms
                .Where(m => !m.isDeleted && m.Id == id)
                .FirstOrDefaultAsync();
        }

        private List<RoomListVM> GetMapDatas(List<Room> rooms)
        {
            List<RoomListVM> roomListVMs = new List<RoomListVM>();

            foreach (var item in rooms)
            {
                RoomListVM newRoom = new RoomListVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Img = item.Image,
                    Price = item.Price,
                };

                roomListVMs.Add(newRoom);
            }

            return roomListVMs;
        }

        private async Task<int> GetPageCount(int take)
        {
            int blogCount = await _context.rooms.Where(m => !m.isDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }
        #endregion
    }
}
