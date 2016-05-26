using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class QuickkeysSetupsViewModel:BaseMobileViewModel
    {
        public QuickkeysSetupsViewModel()
        {
            QuickKeysList= new List<QuickeySetups>();
        }
        public List<QuickeySetups> QuickKeysList { get; set; } 

        public string Planpin { get; set; }
    }
}