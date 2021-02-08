using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions option) : base(option) { }
       
        public DbSet<About> Abouts { get; set; }
        public DbSet<AboutLanguage> AboutLanguages { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberLanguage> MemberLanguages { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceLanguage> ServiceLanguages { get; set; }
        public DbSet<ServicePhoto> ServicePhotos { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SettingLanguage> SettingLanguages { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<TestimonialLanguage> TestimonialLanguages { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Language> Languages { get; set; }

        public async Task SeedAsync(IServiceScope scope)
        {
            if (!this.Languages.Any())
            {
                List<Language> languages = new List<Language>
                {
                    new Language()
                    {
                        Name = "English",
                        Key = "en-US"
                    },
                    new Language()
                    {
                        Name = "Azerbaijan",
                        Key = "az-Latn"
                    }
                };

                await this.Languages.AddRangeAsync(languages);
                await this.SaveChangesAsync();
            }

            if (!this.Users.Any() && !this.Roles.Any())
            {
                AppUser user = new AppUser()
                {
                    Name = "Admin",
                    Surname = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com"
                };
                UserManager<AppUser> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                RoleManager<IdentityRole> _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                await _userManager.CreateAsync(user, "Admin123@");
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await _userManager.AddToRoleAsync(user, "admin");
            }

        }
    }
}