using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TravelApp.Data;

namespace TravelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly IConfiguration _configuration;
        public PhotoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<JsonResult> Upload(List<IFormFile> Photos, string folder)
        {
            if (folder != null && Photos.Count != 0 && Photos != null)
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

                    using (FileStream fs = new FileStream(newPath != null ? newPath : path, FileMode.Create))
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
                return Json(new
                {
                    status = 400
                });

            }
        }

        public async Task<JsonResult> UploadCkEditorPhoto(List<IFormFile> upload, string folder)
        {
            if (folder != null && upload.Count != 0 && upload != null)
            {
                string fileName = "";
                string newPath = null;
                string path = "";
                foreach (IFormFile file in upload)
                {
                     path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", folder, file.FileName);
                    fileName = file.FileName;
                    if (System.IO.File.Exists(path))
                    {
                        Guid guid = Guid.NewGuid();
                        fileName = guid + file.FileName;
                        newPath = path.Replace(file.FileName, fileName);
                    }

                    using (FileStream fs = new FileStream(newPath != null ? newPath : path, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }


                }

                var filePath = _configuration["FileUrl"] +folder+"/"+ fileName;
                return Json(new
                {
                    uploaded = 1,
                    fileName = fileName,
                    url = filePath
                });
            }
            else
            {
                return Json(new
                {
                    uploaded = 0,
                    error = new Dictionary<string, string>() { { "message","Xəta baş verdi."} }
                });

            }
        }
    }
}