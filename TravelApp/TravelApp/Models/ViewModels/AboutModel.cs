using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models.ViewModels
{
    public class AboutModel
    {
        public AboutLanguage AboutLanguage { get; set; }
        public List<MemberLanguage> MemberLanguages { get; set; }
        public List<Client> Clients { get; set; }
        public List<Work> Works { get; set; }
    }
}
