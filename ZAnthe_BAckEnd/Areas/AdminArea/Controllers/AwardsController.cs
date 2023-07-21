using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZAnthe_BAckEnd.Areas.AdminArea.Controllers;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Helpers.Enums;
using ZAnthe_BAckEnd.Helpers;
using ZAnthe_BAckEnd.Models;
using Microsoft.EntityFrameworkCore;
using ZAnthe_BAckEnd.ViewModel.Awards;

namespace ZAnthe_BAckEnd.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class AwardsController : Controller
    {
        #region readonly
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        #endregion

        #region Constructor
        public AwardsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Awards> awards = await _context.awards
                .Where(m => !m.isDeleted)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            ViewBag.take = take;

            List<AwardsListVM> mapDatas = GetMapDatas(awards);

            int count = await GetPageCount(take);

            Paginate<AwardsListVM> result = new Paginate<AwardsListVM>(mapDatas, page, count);

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

                Awards awards = await _context.awards.FirstOrDefaultAsync(m => m.Id == id);

                if (awards is null) return NotFound();

                return View(new AwardsEditVM
                {
                    Title = awards.Name,
                    Image = awards.Img,
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
        public async Task<IActionResult> Update(int id, AwardsEditVM awards)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(awards);
                }
                Awards dbAwards = await GetByIdAsync(id);
                if (awards.Photo != null)
                {
                    if (!awards.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!awards.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + awards.Photo.FileName;
                    Awards awardsDb = await _context.awards.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (awardsDb is null) return NotFound();

                    if (awardsDb.Img == awards.Image)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/allimg", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await awards.Photo.CopyToAsync(stream);
                    }

                    dbAwards.Img = fileName;

                }

                dbAwards.Name = awards.Title;

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
        public async Task<IActionResult> Create(AwardsCreateVM awards)
        {
            if (!ModelState.IsValid) return View();

            if (!awards.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!awards.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + awards.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/awards", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await awards.Photo.CopyToAsync(stream);
            }

            Awards newAwards = new Awards
            {
                Name = awards.Title,
                Img = fileName,
            };

            await _context.awards.AddAsync(newAwards);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            Awards awards = await GetByIdAsync(id);

            if (awards == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", awards.Img);


            Helper.DeleteFile(path);

            _context.awards.Remove(awards);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Services
        private async Task<Awards> GetByIdAsync(int id)
        {
            return await _context.awards.FindAsync(id);
        }

        private List<AwardsListVM> GetMapDatas(List<Awards> awards)
        {
            List<AwardsListVM> awardsListVMs = new List<AwardsListVM>();

            foreach (var item in awards)
            {
                AwardsListVM newAwards = new AwardsListVM
                {
                    Id = item.Id,
                    Title = item.Name,
                    Image = item.Img
                };

                awardsListVMs.Add(newAwards);
            }

            return awardsListVMs;
        }

        private async Task<int> GetPageCount(int take)
        {
            int awardsCount = await _context.awards.Where(m => !m.isDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)awardsCount / take);
        }

        #endregion
    }
}
