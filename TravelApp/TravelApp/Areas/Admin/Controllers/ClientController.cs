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
    [Authorize(Roles="Admin")]
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;
        public ClientController(AppDbContext context)
        {
            _context = context;
        }
        public  async Task<IActionResult> Index()
        {
            List<Client> clients = await _context.Clients.ToListAsync();
            return View(clients);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(List<string> Photos,string Name)
        {
            if (Photos != null && Name!=null)
            {
                Client client = new Client();
                client.Name = Name;
                foreach (string item in Photos)
                {
                    client.Logo = item;
                }

                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Add));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? clientId)
        {
            if (clientId != null)
            {
                Client client = await _context.Clients.Where(w => w.Id == clientId).FirstOrDefaultAsync();
                return View(client);
            }
            return RedirectToAction(nameof(Edit));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<string> Photos, List<string> DeletePhotos, int ClientId,string Name)
        {
            Client client = await _context.Clients.Where(w => w.Id == ClientId).FirstOrDefaultAsync();
            if (!DeletePhotos.IsNull() && !DeletePhotos.IsNullOrEmpty())
            {
                foreach (string deletePhoto in DeletePhotos)
                {
                    if (client.Logo == deletePhoto)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Clients", deletePhoto);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        client.Logo = null;

                    }
                }

            }

            if (!Photos.IsNull() && !Photos.IsNullOrEmpty())
            {
                foreach (string addPhoto in Photos)
                {
                    client.Logo = addPhoto;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> Delete(int? id)
        {
            if (id != null)
            {
                Client client = await _context.Clients
                                                .Where(m => m.Id == id)
                                                .FirstOrDefaultAsync();


                if (client != null)
                {
                    _context.Clients.Remove(client);
                    await _context.SaveChangesAsync();


                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Clients", client.Logo);
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