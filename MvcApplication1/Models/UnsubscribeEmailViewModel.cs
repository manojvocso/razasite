using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class UnsubscribeEmailViewModel : BaseRazaViewModel
    {
        public string EmailId { get; set; }

        public bool Status { get; set; }

        public string Error { get; set; }
    }
}