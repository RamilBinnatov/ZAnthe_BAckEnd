using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Helpers;
using ZAnthe_BAckEnd.Helpers.Enums;
using ZAnthe_BAckEnd.Models;
using ZAnthe_BAckEnd.ViewModel.Slider;

namespace ZAnthe_BAckEnd.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        #region readonly
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        #endregion

        #region Constructor
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Slider> sliders = await _context.sliders
                .Where(m => !m.isDeleted)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            ViewBag.take = take;

            List<SliderListVM> mapDatas = GetMapDatas(sliders);

            int count = await GetPageCount(take);

            Paginate<SliderListVM> result = new Paginate<SliderListVM>(mapDatas, page, count);

            return View(result);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Slider slider = await _context.sliders.FirstOrDefaultAsync(m => m.Id == id);

                if (slider is null) return NotFound();

                return View(new SliderEditVM
                {
                    Title = slider.Title,
                    Header = slider.Header,
                    Image = slider.Img,
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
        public async Task<IActionResult> Update(int id, SliderEditVM slider)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(slider);
                }
                Slider dbSlider = await GetByIdAsync(id);
                if (slider.Photo != null)
                {
                    if (!slider.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!slider.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;
                    Slider sliderDb = await _context.sliders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (sliderDb is null) return NotFound();

                    if (sliderDb.Img == slider.Image)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/allimg", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await slider.Photo.CopyToAsync(stream);
                    }

                    dbSlider.Img = fileName;

                }

                dbSlider.Title = slider.Title;

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

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            if (!ModelState.IsValid) return View();

            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!slider.Photo.CheckFileSize(2000000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/allimg", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(stream);
            }

            Slider newSlider = new Slider
            {
                Title = slider.Title,
                Header = slider.Header,
                Img = fileName,
            };

            await _context.sliders.AddAsync(newSlider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            Slider slider = await GetByIdAsync(id);

            if (slider == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", slider.Img);


            Helper.DeleteFile(path);

            _context.sliders.Remove(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Services
        private async Task<Slider> GetByIdAsync(int id)
        {
            return await _context.sliders.FindAsync(id);
        }

        private List<SliderListVM> GetMapDatas(List<Slider> sliders)
        {
            List<SliderListVM> sliderListVMs = new List<SliderListVM>();

            foreach (var item in sliders)
            {
                SliderListVM newSlider = new SliderListVM
                {
                    Id = item.Id,
                    Title = item.Title,
                    Header = item.Header,
                    Img = item.Img
                };

                sliderListVMs.Add(newSlider);
            }

            return sliderListVMs;
        }

        private async Task<int> GetPageCount(int take)
        {
            int sliderCount = await _context.sliders.Where(m => !m.isDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)sliderCount / take);
        }

        #endregion
    }
}
