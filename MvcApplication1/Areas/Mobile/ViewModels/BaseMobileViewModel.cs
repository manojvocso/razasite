using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class BaseMobileViewModel
    {
        public string Message { get; set; }
        public string MessageType { get; set; }
        public string ReturnUrl { get; set; }
    }
}