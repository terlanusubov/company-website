using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Core.Extensions;
using TravelApp.Data;
using TravelApp.Models;
using TravelApp.Models.ViewModels;

namespace TravelApp.Components
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext db;
        public FooterViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int langId =  await HttpContext.GetCurrentLanguageIdAsync(db,"lang_id");

            SettingLanguage settingLanguage = await db.SettingLanguages
                                                        .Where(sl=>sl.LanguageId == langId)
                                                        .FirstOrDefaultAsync();

            List<ServiceLanguage> serviceLanguages = await db.ServiceLanguages
                                                        .Where(sl => sl.LanguageId == langId)
                                                        .ToListAsync();


            FooterModel model = new FooterModel()
            {
                SettingLanguage = settingLanguage,
                ServiceLanguages = serviceLanguages
            };

            return View(model);
        }
    }
}
