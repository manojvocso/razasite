using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class TopupModel : BaseRazaViewModel
    {
       
        public string TopupAmount { get; set; }
        public string MobileNumber { get; set; }

        public List<State> States { get; set; }

        public SelectList Statelist { get; set; }
        public string State { get; set; }
        public int Country { get; set; }

        public string TopMobileCountry { get; set; }
        public string TopMobileOperator { get; set; }

        public List<MobileTopupOperator> Alloperator { get; set; }

    }

    
}