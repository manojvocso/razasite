using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class FeedbackViewModel : BaseMobileViewModel
    {
        public FeedbackViewModel()
        {
            FeedbackTypeList = new List<string>()
            {
                "About our New Website",
                "I am unable to Login",
                "I am unable to Recharge",
                "Remove Numbers from PinLess SetUp",
                "The Access Number(s)are not working",
                "I want to Cancel My Auto Refill",
                "My Plan is not recharged yet",
                "I was Charged by PAYPAL but order could not be processed",
                "Other Issue"
            };
        }

        public List<string> FeedbackTypeList { get; set; }

        [Required(ErrorMessage = "The firstname is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The lastname is required")]
        public string LastName { get; set; }


        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The type of feedback is required")]
        public string FeedbackType { get; set; }

        [Required(ErrorMessage = "The Feedback is required")]
        public string Feedback { get; set; }
    }
}