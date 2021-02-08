using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Areas.Admin.Models
{
    public class AboutEditViewModel
    {
        public List<Language> Languages { get; set; }
        public List<AboutLanguage> AboutLanguages { get; set; }
    }
}
