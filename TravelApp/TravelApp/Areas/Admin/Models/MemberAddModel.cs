using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Areas.Admin.Models
{
    public class MemberAddModel
    {
        [Required]
        public List<string> Positions { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<string> Photos { get; set; }
        public List<string> DeletePhotos { get; set; }
    }
}
