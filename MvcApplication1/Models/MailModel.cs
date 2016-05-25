using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class MailModel : BaseRazaViewModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ActorName { get; set; }
        public bool TermsnCond { get; set; }
    }
}