using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Core.Filters;
using TravelApp.Data;
using TravelApp.Models;
using TravelApp.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace TravelApp.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext db;
        public ServiceController(AppDbContext _db)
        {
            db = _db;
        }
        [ServiceFilter(typeof(CheckLanguageFilter))]
        public async Task<IActionResult> Index()
        {
            int langId = await HttpContext.GetCurrentLanguageIdAsync(db,"lang_id");

            List<ServiceLanguage> serviceLanguages = await db.ServiceLanguages
                                                                .Where(sl => sl.LanguageId == langId)
                                                                .Include(sl=>sl.Service)
                                                                .ThenInclude(sl=>sl.Photos)
                                                                .ToListAsync();

            return View(serviceLanguages);
        }

        [ServiceFilter(typeof(CheckLanguageFilter))]
        public async Task<IActionResult> SpecificService(int serviceId)
        {
            int langId = await HttpContext.GetCurrentLanguageIdAsync(db, "lang_id");
            ServiceLanguage serviceLanguage = await db.ServiceLanguages
                                                        .Where(sl => sl.LanguageId == langId && sl.ServiceId == serviceId)
                                                        .Include(sl=>sl.Service)
                                                        .ThenInclude(sl=>sl.Photos)
                                                        .FirstOrDefaultAsync();

            return View(serviceLanguage);
        }
    }
}