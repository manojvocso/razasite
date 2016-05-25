using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;


namespace MvcApplication1.Models
{
    public class AutoRefil : BaseRazaViewModel
    {
        public string MemberID { get; set; }
        public string Pin { get; set; }
        public string NameOnCard { get; set; }
        
        public Double ReFill_Amount { get; set; }
        public string IsAutorefillEnroll { get; set; }
        public IList<int> Years { get; set; }
        public List<string> Month { get; set; } 
        public string order_id { get; set; }
     
        public string CreditCardNo { get; set; }
        public string CardType { get; set; }
        public string Planname { get; set; }
        public List<State> StateList { get; set; }
        public List<GetCard> CardList { get; set;}
        public string Exp_Month { get; set; }
        public string Email { get; set; }
        public string Exp_Year { get; set; }
        public string CVV2 { get; set; }
        public string Billing_Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

     
    }
