using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Data;
using TravelApp.Core.Extensions;
using TravelApp.Models;
using Microsoft.EntityFrameworkCore;
using TravelApp.Models.ViewModels;

namespace TravelApp.Components
{
    public class LanguageViewComponent:ViewComponent
    {
        private readonly AppDbContext db;
        public LanguageViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Language language = await HttpContext.GetCurrentLanguageAsync(db, "lang_id");
            List<Language> languages = await db.Languages.ToListAsync();
            LanguageModel model = new LanguageModel()
            {
                CurrentLanguage = language,
                Languages = languages
            };
            return View(model);
        }
    }
}
