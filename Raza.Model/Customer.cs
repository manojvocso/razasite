namespace Raza.Model
{
    public class Customer
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ZipCode { get; set; }

        public static Customer Create(string data)
        {
            var customer = new Customer
                {
                    Address = data.Split('|')[1].Split(',')[4].Split('=')[1],
                    City = data.Split('|')[1].Split(',')[5].Split('=')[1],
                    Country = data.Split('|')[1].Split(',')[8].Split('=')[1],
                    Email = data.Split('|')[1].Split(',')[1].Split('=')[1],
                    FirstName = data.Split('|')[1].Split(',')[2].Split('=')[1],
                    LastName = data.Split('|')[1].Split(',')[3].Split('=')[1],
                    State = data.Split('|')[1].Split(',')[6].Split('=')[1],
                    ZipCode = data.Split('|')[1].Split(',')[7].Split('=')[1]
                };

            return customer;
        }
    }
}
