using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApp.Areas.Admin.Models;
using TravelApp.Core.Filters;
using TravelApp.Data;
using TravelApp.Core.Extensions;
using TravelApp.Models;

namespace TravelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MemberController : Controller
    {
        private readonly AppDbContext _context;
        public MemberController(AppDbContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(CheckLanguageFilter))]
        public async Task<IActionResult> Index()
        {
            int langId = await HttpContext.GetCurrentLanguageIdAsync(_context, "lang_id");
            List<MemberLanguage> memberLanguages = await _context.MemberLanguages
                                                                            .Where(m => m.LanguageId == langId)
                                                                            .Include(m => m.Member)
                                                                            .ToListAsync();

            return View(memberLanguages);
        }

        public async Task<IActionResult> Add()
        {
            List<Language> languages = await _context.Languages.ToListAsync();
            return View(languages);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MemberAddModel model)
        {
            if (ModelState.IsValid)
            {
                Member member = new Member();
                member.Name = model.Name;
                await _context.Members.AddAsync(member);

                List<Language> languages = await _context.Languages.ToListAsync();
                for (int i = 0; i < languages.Count; i++)
                {
                    if (model.Positions[i] != null)
                    {

                        MemberLanguage memberLanguage = new MemberLanguage()
                        {
                            MemberId = member.Id,
                            Position = model.Positions[i],
                            LanguageId = languages[i].Id
                        };
                        await _context.MemberLanguages.AddAsync(memberLanguage);
                    }
                }

                foreach (string file in model.Photos)
                {
                    member.Photo = file;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Add));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? memberId)
        {
            if (memberId != null)
            {
                MemberEditModel model = new MemberEditModel()
                {
                    MemberLanguages = await _context.MemberLanguages
                                                            .Where(ml => ml.MemberId == memberId)
                                                            .Include(s => s.Member)
                                                            .ToListAsync(),
                    Languages = await _context.Languages.ToListAsync()
                };

                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }


        [HttpPost]
        public async Task<IActionResult> Edit(MemberAddModel model, int memberId)
        {
            if (!model.Positions.IsNull() && model.Name != null)
            {
                Member member = await _context.Members.Where(s => s.Id == memberId).FirstOrDefaultAsync();
                member.Name = model.Name;
                List<Language> languages = await _context.Languages.ToListAsync();
                for (int i = 0; i < languages.Count; i++)
                {
                    MemberLanguage MemberLanguage = await _context.MemberLanguages
                                                                    .Where(sl => sl.MemberId == memberId
                                                                        && sl.LanguageId == languages[i].Id)
                                                                    .FirstOrDefaultAsync();

                    if (MemberLanguage == null)
                    {
                        MemberLanguage = new MemberLanguage()
                        {
                            MemberId = memberId,
                            LanguageId = languages[i].Id,
                            Position = model.Positions[i],
                        };

                        await _context.MemberLanguages.AddAsync(MemberLanguage);
                    }
                    else
                    {
                        MemberLanguage.Position = model.Positions[i];
                    }

                }

                if (!model.DeletePhotos.IsNull() && !model.DeletePhotos.IsNullOrEmpty())
                {
                    foreach (string deletePhoto in model.DeletePhotos)
                    {
                        if (member.Photo == deletePhoto)
                        {
                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Members", deletePhoto);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            member.Photo = null;

                        }
                    }

                }

                if (!model.Photos.IsNull() && !model.Photos.IsNullOrEmpty())
                {
                    foreach (string addPhoto in model.Photos)
                    {
                        member.Photo = addPhoto;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Edit));
        }
        public async Task<JsonResult> Delete(int? id)
        {
            if (id != null)
            {
                Member member = await _context.Members
                                                .Where(m => m.Id == id)
                                                .FirstOrDefaultAsync();


                if (member != null)
                {
                    _context.Members.Remove(member);
                    await _context.SaveChangesAsync();


                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Members", member.Photo);
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