using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Core.Extensions;
using TravelApp.Core.Filters;
using TravelApp.Data;
using TravelApp.Models.ViewModels;

namespace TravelApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db)
        {
            db = _db;
        }

        [ServiceFilter(typeof(CheckLanguageFilter))]
        public async Task<IActionResult> Index()
        {
            int langId = await HttpContext.GetCurrentLanguageIdAsync(db, "lang_id");
            HomeModel model = new HomeModel()
            {
                serviceLanguages = await db.ServiceLanguages
                                                .Where(sl=>sl.LanguageId == langId)
                                                .Include(sl=>sl.Service)
                                                .ThenInclude(sl=>sl.Photos)
                                                .ToListAsync(),

                testimonialLanguages = await db.TestimonialLanguages
                                                .Where(tl=>tl.LanguageId == langId)
                                                .Include(tl=>tl.Testimonial)
                                                .ToListAsync()
            };
            return View(model);
        }

    }
}
