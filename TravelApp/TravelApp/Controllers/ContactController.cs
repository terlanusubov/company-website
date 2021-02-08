using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Core.Filters;
using TravelApp.Data;
using TravelApp.Models;
using TravelApp.Models.ViewModels;

namespace TravelApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext db;
        public ContactController(AppDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [ServiceFilter(typeof(CheckLanguageFilter))]
        public IActionResult Index()
        {
            Setting setting = db.Settings.FirstOrDefault();
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                Contact contact = new Contact()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    Phone = model.Phone,
                    CreatedDate = DateTime.Now.ToString()
                };

                await db.Contacts.AddAsync(contact);
                await db.SaveChangesAsync();

                TempData["IsSend"] = "true";
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction(nameof(Index));
        }
    }
}