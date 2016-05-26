using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model.PaymentProcessModel
{
    public class ProcessByUserInfo
    {
        public string MemberId { get; set; }

        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public string UserIpAddress { get; set; }
        
    }
}
