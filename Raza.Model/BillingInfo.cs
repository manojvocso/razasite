namespace Raza.Model
{
    public class BillingInfo
    {
        public string MemberId { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

    
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
        public string RefererEmail { get; set; }
        public string UserType { get; set; }

        public static BillingInfo Create(string data)
        {
            var billInfo = new BillingInfo
            {
                MemberId = data.Split('|')[1].Split(',')[0].Split('=')[1],
                Email = data.Split('|')[1].Split(',')[1].Split('=')[1],
                FirstName = data.Split('|')[1].Split(',')[2].Split('=')[1],
                LastName = data.Split('|')[1].Split(',')[3].Split('=')[1],
                Address = data.Split('|')[1].Split(',')[4].Split('=')[1],
                City = data.Split('|')[1].Split(',')[5].Split('=')[1],
                State = data.Split('|')[1].Split(',')[6].Split('=')[1],
                ZipCode = data.Split('|')[1].Split(',')[7].Split('=')[1],                
                Country = data.Split('|')[1].Split(',')[8].Split('=')[1],
                PhoneNumber = data.Split('|')[1].Split(',')[9].Split('=')[1]
            };

            return billInfo;
        }


        public static BillingInfo CreateProfileInfo(string data)
        {
            var billInfo = new BillingInfo
            {
                MemberId = data.Split('|')[1].Split(',')[0].Split('=')[1],
                Email = data.Split('|')[1].Split(',')[1].Split('=')[1],
                FirstName = data.Split('|')[1].Split(',')[3].Split('=')[1],
                LastName = data.Split('|')[1].Split(',')[4].Split('=')[1],
                Address = data.Split('|')[1].Split(',')[5].Split('=')[1],
                City = data.Split('|')[1].Split(',')[6].Split('=')[1],
                State = data.Split('|')[1].Split(',')[7].Split('=')[1],
                ZipCode = data.Split('|')[1].Split(',')[8].Split('=')[1],
                Country = data.Split('|')[1].Split(',')[9].Split('=')[1],
                PhoneNumber = data.Split('|')[1].Split(',')[10].Split('=')[1]
            };

            return billInfo;
        }

    }


    public class CustomerRegistration
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryPhone { get; set; }
        public string AlternatePhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string AboutUs { get; set; }
        public string RefererEmail { get; set; }
        public string IpAddress { get; set; }

           
  

    }
}
