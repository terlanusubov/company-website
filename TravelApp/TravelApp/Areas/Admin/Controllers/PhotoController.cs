using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Data;

namespace TravelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PhotoController : Controller
    {
        public async Task<JsonResult> Upload(List<IFormFile> Photos,string folder)
        {
            if(folder!=null && Photos.Count != 0 && Photos != null)
            {
                foreach (IFormFile file in Photos)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", folder, file.FileName);
                    string newPath = null;
                    if (System.IO.File.Exists(path))
                    {
                        Guid guid = Guid.NewGuid();
                        newPath = path.Replace(file.FileName, guid + file.FileName);
                    }

                    using (FileStream fs = new FileStream(newPath!=null?newPath:path, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }

                }
                return Json(new
                {
                    status = 200
                });
            }
            else
            {
                return Json(new{
                    status = 400
                });

            }
        }
    }
}