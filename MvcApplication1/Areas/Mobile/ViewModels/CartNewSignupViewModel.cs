using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class CartNewSignupViewModel : BaseMobileViewModel
    {

        [Required(ErrorMessage = "The email is required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email address")]
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The password is required")]
        [MinLength(length: 6, ErrorMessage = "Password should be minimum 6 digits")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirm password is not same")]
        [Required(ErrorMessage = "Please confirm your password")]
        public string ConfirmPassword { get; set; }

        public bool IsDisabledPhoneNumber { get; set; }

    }
}