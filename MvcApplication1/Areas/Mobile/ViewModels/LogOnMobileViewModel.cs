using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class LogOnMobileViewModel : BaseMobileViewModel
    {
        [Required(ErrorMessage = "The email is required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email address")]
        public string UserEmail { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}