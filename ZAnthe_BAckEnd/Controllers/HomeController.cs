using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Models;
using ZAnthe_BAckEnd.Services;
using ZAnthe_BAckEnd.ViewModel.Pages;

namespace ZAnthe_BAckEnd.Controllers
{
    public class HomeController : Controller
    {
        #region Readonly
        private readonly AppDbContext _context;
        private readonly LayoutService _layoutService;
        #endregion

        #region Constructor
        public HomeController(AppDbContext context, LayoutService layoutService)
        {
            _context = context;
            _layoutService = layoutService;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> settingDatas = await _layoutService.GetDatasFromSetting();

            IEnumerable<Slider> sliders = await _context.sliders
                .Where(m => !m.isDeleted)
                .ToListAsync();
            AboutBanner aboutBanners = await _context.aboutBanners
                .Where(m => !m.isDeleted)
                .FirstOrDefaultAsync();
            IEnumerable<Room> rooms = await _context.rooms
                .Where(m => !m.isDeleted)
                .ToListAsync();
            IEnumerable<Service> services = await _context.services
                .Where(m => !m.isDeleted)
                .ToListAsync();
            Poster posters = await _context.posters
                .Where(m => !m.isDeleted)
                .FirstOrDefaultAsync();

            IEnumerable<Country> countries = await _context.countries
                .Where(m => !m.isDeleted)
                .OrderByDescending(m => m.Id)
                .Skip(1)
                .Take(3)
                .ToListAsync();
            Country country = await _context.countries.Where(m => !m.isDeleted).FirstOrDefaultAsync();

            HomeVM model = new HomeVM
            {
                Sliders = sliders,
                aboutBanners = aboutBanners,
                rooms = rooms,
                services = services,
                country = country,
                countries = countries,
                poster = posters,  
            };



            return View(model);
        }
        #endregion


    }
}