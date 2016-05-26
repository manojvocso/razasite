using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class AddNewPinlessNumberViewModel : BaseMobileViewModel
    {
        [Required(ErrorMessage = " ")]
        public string Number1 { get; set; }

        [Required(ErrorMessage = " ")]
        public string Number2 { get; set; }

        [Required(ErrorMessage = " ")]
        //[Range(3, 3, ErrorMessage = "Please enter a 10 digit valid pinless number")]
        public string Number3 { get; set; }

        
        [Required(ErrorMessage = "Please enter a calling from number")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Please enter a 10 digit valid pinless number")]
        public string NewPinlessNumber { get; set; }

        public string PlanPin { get; set; }
    }
}