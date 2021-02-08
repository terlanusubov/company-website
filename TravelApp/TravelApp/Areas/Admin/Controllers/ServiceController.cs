using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApp.Areas.Admin.Models;
using TravelApp.Core.Extensions;
using TravelApp.Core.Filters;
using TravelApp.Data;
using TravelApp.Models;

namespace TravelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly AppDbContext db;
        public ServiceController(AppDbContext _db)
        {
            db = _db;
        }
        [ServiceFilter(typeof(CheckLanguageFilter))]
        public async Task<IActionResult> Index()
        {
            int langId = Convert.ToInt32(await HttpContext.GetCurrentLanguageIdAsync(db, "lang_id"));
            List<ServiceLanguage> serviceLanguages = await db.ServiceLanguages
                                                                    .Where(sl => sl.LanguageId == langId)
                                                                    .Include(sl => sl.Service)
                                                                    .ThenInclude(s => s.Photos)
                                                                    .ToListAsync();


            return View(serviceLanguages);
        }
        public async Task<IActionResult> Add()
        {
            List<Language> languages = await db.Languages.ToListAsync();
            return View(languages);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ServiceModel model)
        {

            if( model.Names!= null && 
                model.Photos != null &&
                model.Texts!=null &&
                model.ShortDescs != null)
            {
                Service service = new Service();
                await db.Services.AddAsync(service);

                List<Language> languages = await db.Languages.ToListAsync();
                for (int i = 0; i < languages.Count; i++)
                {
                    if(model.Names[i]!=null && model.ShortDescs[i]!=null && model.Texts[i] != null)
                    {

                        ServiceLanguage serviceLanguage = new ServiceLanguage()
                        {
                            Name = model.Names[i],
                            ShortDesc = model.ShortDescs[i],
                            Text = model.Texts[i],
                            ServiceId = service.Id,
                            LanguageId = languages[i].Id
                        };
                        await db.ServiceLanguages.AddAsync(serviceLanguage);
                    }
                }

                foreach (string file in model.Photos)
                {
                    ServicePhoto servicePhoto = new ServicePhoto()
                    {
                        ServiceId = service.Id,
                        Path = file
                    };
                    await db.ServicePhotos.AddAsync(servicePhoto);
                }
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Add));
            }
        }
        [HttpPost]
        public async Task<JsonResult> Delete(int? id)
        {
            if (id != null)
            {
                Service service = await db.Services
                                                .Where(s => s.Id == id)
                                                .Include(s=>s.Photos)
                                                .FirstOrDefaultAsync();


                if (service != null)
                {
                    db.Services.Remove(service);
                    await db.SaveChangesAsync();


                    List<string> photos = service.Photos.Select(sp => sp.Path).ToList();
                    foreach (string photo in photos)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Services", photo);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
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

        [HttpGet]
        public async Task<IActionResult> Edit(int? serviceId)
        {
            if (serviceId != null)
            {
                ServiceEditViewModel model = new ServiceEditViewModel()
                {
                    ServiceLanguages = await db.ServiceLanguages
                                                            .Where(sl => sl.ServiceId == serviceId)
                                                            .Include(s => s.Service)
                                                            .ThenInclude(s => s.Photos)
                                                            .ToListAsync(),
                    Languages = await db.Languages.ToListAsync()
                };

                return View(model);

            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ServiceModel model, int serviceId)
        {
            if (!model.Texts.IsNull() && !model.Names.IsNull() && !model.ShortDescs.IsNull())
            {
                Service service = await db.Services.Where(s => s.Id == serviceId).Include(s => s.Photos).FirstOrDefaultAsync();
                List<Language> languages = await db.Languages.ToListAsync();


                for (int i = 0; i < languages.Count; i++)
                {
                    ServiceLanguage serviceLanguage = await db.ServiceLanguages
                                                                    .Where(sl => sl.ServiceId == serviceId
                                                                        && sl.LanguageId == languages[i].Id)
                                                                    .FirstOrDefaultAsync();

                    if (serviceLanguage == null)
                    {
                        serviceLanguage = new ServiceLanguage()
                        {
                            ServiceId = serviceId,
                            LanguageId = languages[i].Id,
                            Name = model.Names[i],
                            Text = model.Texts[i],
                            ShortDesc = model.ShortDescs[i]
                        };

                        await db.ServiceLanguages.AddAsync(serviceLanguage);
                    }
                    else
                    {
                        serviceLanguage.Name = model.Names[i];
                        serviceLanguage.ShortDesc = model.ShortDescs[i];
                        serviceLanguage.Text = model.Texts[i];
                    }

                }

                if (!model.DeletePhotos.IsNull() && !model.DeletePhotos.IsNullOrEmpty())
                {

                    foreach (ServicePhoto sp in service.Photos)
                    {
                        if (model.DeletePhotos.Contains(sp.Path))
                        {
                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Uploads", "Services", sp.Path);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            ServicePhoto servicePhoto = await db.ServicePhotos.Where(s => s.ServiceId == serviceId && s.Path == sp.Path).FirstOrDefaultAsync();
                            if (servicePhoto != null)
                            {
                                db.ServicePhotos.Remove(servicePhoto);
                            }
                        }
                    }
                }

                if (!model.Photos.IsNull() && !model.Photos.IsNullOrEmpty())
                {
                    foreach (string file in model.Photos)
                    {
                        ServicePhoto servicePhoto = new ServicePhoto()
                        {
                            ServiceId = service.Id,
                            Path = file
                        };
                        await db.ServicePhotos.AddAsync(servicePhoto);
                    }
                }


                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Edit));
        }
    }
}