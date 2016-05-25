using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class SaveFeedback
    {
        public bool IsRazaCustomer { get; set; }
        public string FeedbackType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Feedback { get; set; }

    }
}