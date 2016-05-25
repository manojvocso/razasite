using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;


namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class PinlessSetupViewModel:BaseMobileViewModel
    {
        public PinlessSetupViewModel()
        {
            RegisteredPinlessNumbers = new List<PinLessSetupNumbers>();
        }
        public List<PinLessSetupNumbers> RegisteredPinlessNumbers { get; set; }
        public string OrderId { get; set; }
        public string PlanPin { get; set; }
    }
}