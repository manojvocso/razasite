using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class WhyRaza : BaseRazaViewModel
    {
        public string Email { get; set; }
        public string mail1 { get; set; }
        public string mail2 { get; set; }
        public string mail3 { get; set; }
        public string mail4 { get; set; }
        public string mail5 { get; set; }
        public List<string> Mail { get; set; }
        public List<string> ReferFriendName { get; set; }


        public List<WhyRaza> Message { get; set; } 
        public string Msg { get; set; }
    }
}