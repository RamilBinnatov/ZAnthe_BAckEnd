using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ZAnthe_BAckEnd.Data;
using ZAnthe_BAckEnd.Models;
using ZAnthe_BAckEnd.ViewModel.Contact;

namespace ZAnthe_BAckEnd.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

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
        public async Task<IActionResult> Index(ContactUs contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }

                bool isExsist = await _context.contactUs.AnyAsync(m => m.Name.Trim() == contact.Name.Trim()
                      && m.Email.Trim() == contact.Email.Trim()
                      && m.Subject.Trim() == contact.Subject.Trim()
                      && m.Message.Trim() == contact.Message.Trim());



                if (isExsist)
                {
                    ModelState.AddModelError("Name", "Subject is already exist");
                    return View();
                }
                await _context.contactUs.AddAsync(contact);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {

                return View();
            }
        }
    }
}

