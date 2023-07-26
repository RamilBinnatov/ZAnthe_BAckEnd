using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Helpers;
using ZAnthe_BAckEnd.Models;
using ZAnthe_BAckEnd.ViewModel.Services;
using ZAnthe_BAckEnd.ViewModel.Slider;

namespace ZAnthe_BAckEnd.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ServiceController : Controller
    {
        #region readonly
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        #endregion

        #region Constructor
        public ServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Service> services = await _context.services
                .Where(m => !m.isDeleted)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            ViewBag.take = take;

            List<ServicesListVM> mapDatas = GetMapDatas(services);

            int count = await GetPageCount(take);

            Paginate<ServicesListVM> result = new Paginate<ServicesListVM>(mapDatas, page, count);

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
        public async Task<IActionResult> Create(ServiceCreateVM service)
        {
            if (!ModelState.IsValid) return View();

            if (!service.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!service.Photo.CheckFileSize(2000000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + service.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/allimg", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await service.Photo.CopyToAsync(stream);
            }

            Service newService = new Service
            {
                Title = service.Title,
                Desc = service.Desc,
                Image = fileName,
            };

            await _context.services.AddAsync(newService);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            Service services = await GetByIdAsync(id);

            if (services == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", services.Image);


            Helper.DeleteFile(path);

            _context.services.Remove(services);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Services
        private async Task<Service> GetByIdAsync(int id)
        {
            return await _context.services.FindAsync(id);
        }

        private List<ServicesListVM> GetMapDatas(List<Service> services)
        {
            List<ServicesListVM> servicesListVMs = new List<ServicesListVM>();

            foreach (var item in services)
            {
                ServicesListVM newService = new ServicesListVM
                {
                    Id = item.Id,
                    Title = item.Title,
                    Desc = item.Desc,
                    Img = item.Image
                };

                servicesListVMs.Add(newService);
            }

            return servicesListVMs;
        }

        private async Task<int> GetPageCount(int take)
        {
            int serviceCount = await _context.services.Where(m => !m.isDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)serviceCount / take);
        }

        #endregion
    }
}
