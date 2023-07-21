using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Models;
using ZAnthe_BAckEnd.ViewModel.Contact;
using static System.Collections.Specialized.BitVector32;

namespace ZAnthe_BAckEnd.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            


           

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactVM contact)
        {
            if (!ModelState.IsValid) return View();

            ContactUs contacts = new ContactUs
            {
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
            };

            await _context.contactUs.AddAsync(contacts);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}



