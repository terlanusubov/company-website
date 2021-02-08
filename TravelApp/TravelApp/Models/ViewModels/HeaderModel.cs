using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models.ViewModels
{
    public class HeaderModel
    {
        public SettingLanguage SettingLanguage { get; set; }
        public List<ServiceLanguage> ServiceLanguages { get; set; }
    }
}
