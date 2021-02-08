using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class ServiceLanguage
    {
        public int Id { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Text { get; set; }
    }
}
