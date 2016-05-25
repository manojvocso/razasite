using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class CallDetailsViewModel : BaseMobileViewModel
    {
        public CallDetailsViewModel()
        {
            AllCalls= new List<EachCall>();
        }
        public List<EachCall> AllCalls { get; set; }
        public string PlanName { get; set; }
        public string PlanPin { get; set; }
        public string OrderId { get; set; }
    }
}