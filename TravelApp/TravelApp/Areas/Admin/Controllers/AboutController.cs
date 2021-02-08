using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApp.Areas.Admin.Models;
using TravelApp.Core.Extensions;
using TravelApp.Data;
using TravelApp.Models;

namespace TravelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        public AboutController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            int langId = await HttpContext.GetCurrentLanguageIdAsync(_context, "lang_id");
            List<AboutLanguage> aboutLanguages = await _context.AboutLanguages.Where(a => a.LanguageId == langId).ToListAsync();
            return View(aboutLanguages);
        }

        public async Task<IActionResult> Add()
        {
            List<Language> languages = await _context.Languages.ToListAsync();
            return View(languages);
        }

        [HttpPost]
        public async Task<IActionResult> Add(List<string> Texts)
        {
            if (Texts != null)
            {
                About about = new About();
                await _context.Abouts.AddAsync(about);

                List<Language> languages = await _context.Languages.ToListAsync();
                for (int i = 0; i < languages.Count; i++)
                {
                    if (Texts[i] != null)
                    {

                        AboutLanguage aboutLanguage = new AboutLanguage()
                        {
                            AboutId = about.Id,
                            Text = Texts[i],
                            LanguageId = languages[i].Id
                        };
                        await _context.AboutLanguages.AddAsync(aboutLanguage);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Add));
        }

        public async Task<IActionResult> Edit(int aboutId)
        {
            if (aboutId != 0)
            {
                List<AboutLanguage> aboutLanguages = await _context.AboutLanguages.Where(al => al.AboutId == aboutId).ToListAsync();
                List<Language> languages = await _context.Languages.ToListAsync();


                AboutEditViewModel model = new AboutEditViewModel()
                {
                    Languages = languages,
                    AboutLanguages = aboutLanguages
                };
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<string> Texts,int AboutId)
        {
            List<Language> languages = await _context.Languages.ToListAsync();
            for (int i = 0; i < languages.Count; i++)
            {
                AboutLanguage aboutLanguage = await _context.AboutLanguages
                                                                .Where(sl => sl.AboutId == AboutId
                                                                    && sl.LanguageId == languages[i].Id)
                                                                .FirstOrDefaultAsync();

                if (aboutLanguage == null)
                {
                    AboutLanguage about = new AboutLanguage()
                    {
                        AboutId = AboutId,
                        LanguageId = languages[i].Id,
                        Text =Texts[i],
                    };

                    await _context.AboutLanguages.AddAsync(about);
                }
                else
                {
                    aboutLanguage.Text = Texts[i];
                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> Delete(int? id)
        {
            if (id != null)
            {
                About about = await _context.Abouts
                                                .Where(m => m.Id == id)
                                                .FirstOrDefaultAsync();


                if (about != null)
                {
                    _context.Abouts.Remove(about);
                    await _context.SaveChangesAsync();

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