using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelApp.Core.Filters;
using TravelApp.Data;
using TravelApp.Models;

namespace TravelApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configure Localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                       new CultureInfo("en-US"),
                       new CultureInfo("az-Latn")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0].ToString(), uiCulture: supportedCultures[0].ToString());
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

            });


            //Filter
            services.AddScoped<CheckLanguageFilter>();

            //Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddAuthentication();

            services.AddMvc()
                //View Localization
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(Configuration["Database:Connection"]);
            });

            services.AddIdentity<AppUser, IdentityRole>()
                                .AddDefaultTokenProviders()
                                    .AddEntityFrameworkStores<AppDbContext>();


            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Admin/Account/Login";
                option.AccessDeniedPath = "/Home/Error";

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                 name: "areas",
                 template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
           });
        }
    }
}
