using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Core.Filters;
using TravelApp.Data;
using TravelApp.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using TravelApp.Models;
using TravelApp.Models.ViewModels;

namespace TravelApp.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext db;
        public AboutController(AppDbContext _db)
        {
            db = _db;
        }

        [ServiceFilter(typeof(CheckLanguageFilter))]
        public async Task<IActionResult> Index()
        {
            int langId = await HttpContext.GetCurrentLanguageIdAsync(db, "lang_id");
            AboutModel model = new AboutModel()
            {
                AboutLanguage = await db.AboutLanguages
                                                    .Where(al => al.LanguageId == langId)
                                                    .Include(al => al.About)
                                                    .FirstOrDefaultAsync(),
                MemberLanguages = await db.MemberLanguages.Include(m=>m.Member).ToListAsync(),
                Clients = await db.Clients.ToListAsync(),
                Works = await db.Works.ToListAsync()
            };
             
            return View(model);
        }
    }
}