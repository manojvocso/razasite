using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class DomesticTopupViewModel : BaseMobileViewModel
    {
        public DomesticTopupViewModel()
        {
            CarrierList = new List<MobileTopupOperator>();
            DenominationList = new List<MobileTopupOperator>();
        }

        public List<MobileTopupOperator> CarrierList { get; set; }
        public List<MobileTopupOperator> DenominationList { get; set; }


        public double DestinationAmount { get; set; }

        [Required(ErrorMessage = "Please select a carrier")]
        public double SourceAmount { get; set; }

        public string OperatorCode { get; set; }
        
        [Required(ErrorMessage = "Please select a carrier")]
        public string CarrierName { get; set; }

        
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "The Mobile number is required")]
        [MaxLength(10,ErrorMessage = "The phone number should be 10 digit.")]
        public string MobileNumber { get; set; }
    }
}