using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Core.Extensions;
using TravelApp.Data;

namespace TravelApp.Controllers
{
    public class LanguageController : Controller
    {
        private readonly AppDbContext db;
        public LanguageController(AppDbContext _db)
        {
            db = _db;
        }
        [HttpPost]
        public async Task<IActionResult> SetLanguage(string culture, string returnUrl)
        {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                 );
                await HttpContext.SetLanguageAsync(db,"lang_id",culture);
                return LocalRedirect(returnUrl);
        }
    }
}