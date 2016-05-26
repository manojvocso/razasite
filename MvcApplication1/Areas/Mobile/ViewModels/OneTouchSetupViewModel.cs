using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class OneTouchSetupViewModel : BaseMobileViewModel
    {
        public List<OneTouchSetups> RegisteredOnetouchs { get; set; }
        public string PlanPin { get; set; }
    }
}