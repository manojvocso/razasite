using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class CCVerifyModel : BaseRazaViewModel
    {
        public string RedirectUrl { get; set; }
        public string Message { get; set; }
    }
}