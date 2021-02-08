using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class MemberLanguage
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public Member Member { get; set; }
        public int MemberId { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; }
    }
}
