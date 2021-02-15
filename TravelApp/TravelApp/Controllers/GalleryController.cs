using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Data;

namespace TravelApp.Controllers
{
    public class GalleryController : Controller
    {
        private readonly AppDbContext _db;
        public GalleryController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Works.ToListAsync());
        }
    }
}
