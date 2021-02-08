using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models.ViewModels
{
    public class LanguageModel
    {
        public List<Language> Languages { get; set; }
        public Language CurrentLanguage { get; set; }
    }
}
