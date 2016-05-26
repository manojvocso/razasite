using Raza.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;
using MvcApplication1.AppHelper;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class BillingInfoViewModel : BaseMobileViewModel
    {
        public BillingInfoViewModel()
        {
            States = new List<State>();
            Countries = new List<Country>();
        }
        public string MemberId { get; set; }

        [Required(ErrorMessage = "The first name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The city is required.")]
        public string City { get; set; }

        //[Required(ErrorMessage = "State is required.")]
        [RequiredIfNot("Country", 3, ErrorMessage = "The State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "The zipCode is required.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "The phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The country is required.")]
        public string Country { get; set; }

        public string Email { get; set; }

        [RequiredIfNotEmpty("NewPwd", ErrorMessage = "The old password is required.")]
        public string OldPwd { get; set; }

        [RequiredIfNotEmpty("OldPwd", ErrorMessage = "The new password is required")]
        public string NewPwd { get; set; }

        [RequiredIfNotEmpty("OldPwd", ErrorMessage = "The confirm password is required")]
        [Compare("NewPwd", ErrorMessage = "New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email address")]
        public string RefrerEmail { get; set; }

        public bool IsValidForReferer { get; set; }

        public string UserType { get; set; }
        public List<State> States { get; set; }
        public List<Country> Countries { get; set; }
        public bool IsExpand { get; set; }
        
    }
}