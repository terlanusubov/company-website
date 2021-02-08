using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Areas.Admin.Models;
using TravelApp.Models;

namespace TravelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        public AccountController(UserManager<AppUser> _userManager,
                                 SignInManager<AppUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Error");
                    return View();
                }
                else
                {
                    bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                    if (isAdmin)
                    {
                        var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                        if (signInResult.Succeeded)
                        {
                            HttpContext.Session.SetString("admin_name", user.Name);
                            HttpContext.Session.SetString("admin_surname", user.Surname);
                            HttpContext.Session.SetString("admin_email", user.Email);
                            HttpContext.Session.SetString("admin_id", user.Id);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return View();
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            else
            {
                return View();
            }
        }
    }
}