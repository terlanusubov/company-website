using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Areas.Admin.Models
{
    public class ServiceModel
    {
        public List<string> Names { get; set; }
        public List<string> ShortDescs { get; set; }
        public List<string> Photos { get; set; }
        public List<string> Texts { get; set; }
        public List<string> DeletePhotos { get; set; }

    }
}
