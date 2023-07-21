using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Helpers.Enums;
using ZAnthe_BAckEnd.Helpers;
using ZAnthe_BAckEnd.Models;
using Microsoft.EntityFrameworkCore;
using ZAnthe_BAckEnd.ViewModel.RoomService;

namespace ZAnthe_BAckEnd.Areas.AdminArea.Controllers
{

        [Area("Adminarea")]
        public class RoomServiceController : Controller
        {
            #region readonly
            private readonly AppDbContext _context;
            private readonly IWebHostEnvironment _env;
            #endregion

            #region Constructor
            public RoomServiceController(AppDbContext context, IWebHostEnvironment env)
            {
                _context = context;
                _env = env;
            }
            #endregion

            #region Index
            public async Task<IActionResult> Index(int page = 1, int take = 5)
            {
                List<RoomService> services = await _context.roomServices
                    .Where(m => !m.isDeleted)
                .Skip((page * take) - take)
                    .Take(take)
                    .ToListAsync();
                ViewBag.take = take;
                List<RoomServiceListVM> mapDatas = GetMapDatas(services);
                int count = await GetPageCount(take);

                Paginate<RoomServiceListVM> result = new Paginate<RoomServiceListVM>(mapDatas, page, count);

                return View(result);
            }
            #endregion

            #region Create

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(RoomServiceCreateVM serviceVM)
            {
                if (!ModelState.IsValid) return View();

                RoomService newService = new RoomService
                {
                    Name = serviceVM.Name,
                    Img = serviceVM.Img,
                };

                await _context.roomServices.AddAsync(newService);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            #endregion

            #region Delete
            [HttpPost]
            public async Task<IActionResult> Delete(int id)
            {
                RoomService service = await _context.roomServices.FirstOrDefaultAsync(m => m.Id == id);

                service.isDeleted = true;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            #endregion

            #region Edit
            [HttpGet]
            public async Task<IActionResult> Update(int? id)
            {
                try
                {
                    if (id is null) return BadRequest();

                    RoomService roomService = await _context.roomServices.FirstOrDefaultAsync(m => m.Id == id);

                    if (roomService is null) return NotFound();

                    return View(new RoomService
                    {
                        Name = roomService.Name,
                        Img = roomService.Img,
                    });

                }
                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message;
                    return View();
                }
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Update(int id, RoomServiceEditVM serviceEditVM)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return View(serviceEditVM);
                    }
                    RoomService dbRoomService = await GetByIdAsync(id);


                    dbRoomService.Name = serviceEditVM.Name;
                    dbRoomService = await GetByIdAsync(id);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {

                    ViewBag.Message = ex.Message;
                    return View();
                }
            }
            #endregion

            #region Services
            private async Task<RoomService> GetByIdAsync(int id)
            {
                return await _context.roomServices.FindAsync(id);
            }

            private List<RoomServiceListVM> GetMapDatas(List<RoomService> services)
            {
                List<RoomServiceListVM> roomServiceListVMs = new List<RoomServiceListVM>();

                foreach (var item in services)
                {
                    RoomServiceListVM newServices = new RoomServiceListVM
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Img = item.Img
                    };

                    roomServiceListVMs.Add(newServices);
                }

                return roomServiceListVMs;
            }

            private async Task<int> GetPageCount(int take)
            {
                int serviceCount = await _context.roomServices.Where(m => !m.isDeleted).CountAsync();

                return (int)Math.Ceiling((decimal)serviceCount / take);
            }

            #endregion
        }
    }

