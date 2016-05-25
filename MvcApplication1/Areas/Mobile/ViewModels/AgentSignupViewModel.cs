using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class AgentSignupViewModel : BaseMobileViewModel
    {
        [Required(ErrorMessage = "The firstname is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The lastname is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The email is required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The message is required")]
        public string AgentMessage { get; set; }

    }
}