using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Areas.Admin.Models
{
    public class ServiceEditViewModel
    {
        public List<ServiceLanguage> ServiceLanguages { get; set; }
        public List<Language> Languages { get; set; }
    }
}
