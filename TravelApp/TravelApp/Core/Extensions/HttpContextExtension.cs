using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Data;
using TravelApp.Models;

namespace TravelApp.Core.Extensions
{
    public static class HttpContextExtension
    {
        public async static Task<Language> GetCurrentLanguageAsync(this HttpContext context,AppDbContext db,string key)
        {
            int langId = Convert.ToInt32(context.Session.GetString(key));
            Language language = await db.Languages.Where(l => langId != 0 ? (l.Id == langId) : true).FirstOrDefaultAsync();
            return language;
        }
        public async static Task<int> GetCurrentLanguageIdAsync(this HttpContext context,AppDbContext db,string key)
        {
            int langId = Convert.ToInt32(context.Session.GetString(key));
            Language language = await db.Languages.Where(l => langId != 0 ? (l.Id == langId) : true).FirstOrDefaultAsync();

            return language.Id;
        }
        public async static Task SetLanguageAsync(this HttpContext context,AppDbContext db,string key,string culture)
        {
            Language language = await db.Languages.Where(l => culture!=null?(l.Key == culture):true).FirstOrDefaultAsync();
            context.Session.SetString(key, language.Id.ToString());
        }
    }
}
