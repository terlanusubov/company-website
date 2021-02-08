using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class Service
    {
        public int Id { get; set; }
        public List<ServicePhoto> Photos { get; set; }
    }
}
