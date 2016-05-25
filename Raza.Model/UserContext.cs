using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class UserContext
    {
        public string MemberId { get; set; }

        public bool IsEmailSubscribed { get; set; }

        public string Pin { get; set; }

        public string Email { get; set; }

        public string UserType { get; set; }

        public string ServiceResponse { get; set; }

        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                {
                    return string.Format("{0} {1}", char.ToUpper(FirstName[0]), LastName);
                }
                else if (!string.IsNullOrEmpty(FirstName))
                {
                    return string.Format("{0} ", char.ToUpper(FirstName[0]));
                }
                else if(!string.IsNullOrEmpty(LastName))
                {
                    return LastName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string FirstName
        {
            get { return ProfileInfo != null ? ProfileInfo.FirstName : string.Empty; }
        }

        public string LastName
        {
            get { return ProfileInfo != null ? ProfileInfo.FirstName : string.Empty; }
        }

        public BillingInfo ProfileInfo { get; set; }
    }
}
