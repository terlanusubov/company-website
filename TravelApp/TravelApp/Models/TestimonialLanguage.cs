using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class TestimonialLanguage
    {
        public int Id { get; set; }
        public Testimonial Testimonial { get; set; }
        public int TestimonialId { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; }
        public string Text { get; set; }
    }
}
