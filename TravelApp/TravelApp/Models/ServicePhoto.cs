using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class ServicePhoto
    {
        public int Id { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }
        public string Path { get; set; }

    }
}
