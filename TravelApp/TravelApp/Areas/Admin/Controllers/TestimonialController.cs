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
    [Authorize(Roles="Admin")]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _context;
        public TestimonialController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            int langId = await HttpContext.GetCurrentLanguageIdAsync(_context, "lang_id");
            List<TestimonialLanguage> testimonialLanguages = await _context.TestimonialLanguages.Where(l => l.LanguageId == langId).Include(t=>t.Testimonial).ToListAsync();
            return View(testimonialLanguages);
        }

        public async Task<IActionResult> Add()
        {
            List<Language> languages = await _context.Languages.ToListAsync();
            return View(languages);
        }

        [HttpPost]
        public async Task<IActionResult> Add(List<string> Texts,string Name)
        {
            if (Texts != null && Name!=null)
            {
                Testimonial testimonial = new Testimonial();
                testimonial.Name = Name;
                await _context.Testimonials.AddAsync(testimonial);

                List<Language> languages = await _context.Languages.ToListAsync();
                for (int i = 0; i < languages.Count; i++)
                {
                    if (Texts[i] != null)
                    {

                        TestimonialLanguage testimonialLanguage = new TestimonialLanguage()
                        {
                            TestimonialId = testimonial.Id,
                            Text = Texts[i],
                            LanguageId = languages[i].Id
                        };
                        await _context.TestimonialLanguages.AddAsync(testimonialLanguage);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Add));
        }

        public async Task<IActionResult> Edit(int testimonialId)
        {
            if (testimonialId != 0)
            {
                List<TestimonialLanguage> testimonialLanguages = await _context.TestimonialLanguages.Where(al => al.TestimonialId == testimonialId).Include(t=>t.Testimonial).ToListAsync();
                List<Language> languages = await _context.Languages.ToListAsync();


                TestimonialEditViewModel model = new TestimonialEditViewModel()
                {
                    Languages = languages,
                    TestimonialLanguages = testimonialLanguages
                };
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<string> Texts, int TestimonialId,string Name)
        {
            List<Language> languages = await _context.Languages.ToListAsync();
            Testimonial testimonial1 = await _context.Testimonials.Where(t => t.Id == TestimonialId).FirstOrDefaultAsync();
            testimonial1.Name = Name;
            for (int i = 0; i < languages.Count; i++)
            {
                TestimonialLanguage testimonialLanguage = await _context.TestimonialLanguages
                                                                .Where(sl => sl.TestimonialId == TestimonialId
                                                                    && sl.LanguageId == languages[i].Id)
                                                                .FirstOrDefaultAsync();

                if (testimonialLanguage == null)
                {
                    TestimonialLanguage testimonial = new TestimonialLanguage()
                    {
                        TestimonialId = TestimonialId,
                        LanguageId = languages[i].Id,
                        Text = Texts[i],
                    };

                    await _context.TestimonialLanguages.AddAsync(testimonial);
                }
                else
                {
                    testimonialLanguage.Text = Texts[i];
                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> Delete(int? id)
        {
            if (id != null)
            {
                Testimonial testimonial = await _context.Testimonials
                                                .Where(m => m.Id == id)
                                                .FirstOrDefaultAsync();


                if (testimonial != null)
                {
                    _context.Testimonials.Remove(testimonial);
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