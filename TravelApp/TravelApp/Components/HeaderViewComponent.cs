using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Data;
using TravelApp.Models;
using TravelApp.Core.Extensions;
using TravelApp.Models.ViewModels;

namespace TravelApp.Components
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext db;
        public HeaderViewComponent(AppDbContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int langId = await HttpContext.GetCurrentLanguageIdAsync(db,"lang_id");

            SettingLanguage settingLanguage = await db.SettingLanguages
                                                            .Where(sl => sl.LanguageId == langId)
                                                            .Include(sl => sl.Setting)
                                                            .FirstOrDefaultAsync();

            List<ServiceLanguage> serviceLanguages = await db.ServiceLanguages
                                                                .Where(sl => sl.LanguageId == langId)
                                                                .Include(sl => sl.Service)
                                                                .ToListAsync();


            HeaderModel model = new HeaderModel()
            {
                SettingLanguage = settingLanguage,
                ServiceLanguages = serviceLanguages
            };

            return View(model);
        }
    }
}
