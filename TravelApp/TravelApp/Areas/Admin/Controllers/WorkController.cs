using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApp.Core.Extensions;
using TravelApp.Data;
using TravelApp.Models;

namespace TravelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WorkController : Controller
    {
        private readonly AppDbContext _context;
        public WorkController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Work> works = await _context.Works.ToListAsync();

            return View(works);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(List<string> Photos)
        {
            if (Photos != null)
            {
                Work work = new Work();
                foreach (string item in Photos)
                {
                    work.Photo = item;
                }

                await _context.Works.AddAsync(work);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Add));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? workId)
        {
            if(workId != null)
            {
                Work work = await _context.Works.Where(w => w.Id == workId).FirstOrDefaultAsync();
                return View(work);
            }
            return RedirectToAction(nameof(Edit));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<string> Photos,List<string> DeletePhotos,int WorkId)
        {
            Work work = await _context.Works.Where(w => w.Id == WorkId).FirstOrDefaultAsync();
            if (!DeletePhotos.IsNull() && !DeletePhotos.IsNullOrEmpty())
            {
                foreach (string deletePhoto in DeletePhotos)
                {
                    if (work.Photo == deletePhoto)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Works", deletePhoto);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        work.Photo = null;

                    }
                }

            }

            if (!Photos.IsNull() && !Photos.IsNullOrEmpty())
            {
                foreach (string addPhoto in Photos)
                {
                    work.Photo = addPhoto;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> Delete(int? id)
        {
            if (id != null)
            {
                Work work = await _context.Works
                                                .Where(m => m.Id == id)
                                                .FirstOrDefaultAsync();


                if (work != null)
                {
                    _context.Works.Remove(work);
                    await _context.SaveChangesAsync();


                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Works", work.Photo);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    return Json(new
                    {
                        status = 200
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = 400
                    });
                }
            }
            else
            {
                return Json(new
                {
                    status = 400
                });
            }
        }
    }
}