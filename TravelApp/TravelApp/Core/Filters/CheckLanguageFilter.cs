using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Core.Extensions;
using TravelApp.Data;

namespace TravelApp.Core.Filters
{
    public class CheckLanguageFilter : IActionFilter
    {
        private readonly AppDbContext db;
        public CheckLanguageFilter(AppDbContext _db)
        {
            db = _db;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
             int langId= Convert.ToInt32(context.HttpContext.Session.GetString("lang_id"));
              if (langId == 0)
              {
                  context.HttpContext.SetLanguageAsync(db, "lang_id", null).Wait();
              }

        }
    }
}
