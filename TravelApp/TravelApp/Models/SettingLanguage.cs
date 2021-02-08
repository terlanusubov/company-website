using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class SettingLanguage
    {
        public int Id { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; }
        public Setting Setting { get; set; }
        public int SettingId { get; set; }
        public string Adress { get; set; }
        public string AboutUs { get; set; }
        public string ShowroomHours { get; set; }
    }
}
