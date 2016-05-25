using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class CheckOutModel : BaseRazaViewModel
    {
        public List<State> States { get; set; }

        public SelectList Statelist { get; set; }

        public List<Country> Countries { get; set; }

        public string MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string UserName
        {
            get
            {
                return FirstName + "" + LastName;
            }

        }

        public string PlanName { get; set; }
        public double Amount { get; set; }
        public double ServiceFee { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string CvvNumber { get; set; }
        public string Pin { get; set; }
        public string PaymentMethod { get; set; }
        public string CoupanCode { get; set; }
        public string IpAddress { get; set; }
        public string ExpDate { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string CardType { get; set; }
        public int CardId { get; set; }
        public string UserType { get; set; }
        public string AniNumber { get; set; }
        public string IsPasscodeDial { get; set; }
        public string PassCodePin { get; set; }
        public string PaymentType { get; set; }
        public string autoRefillAmount { get; set; }
        public string autoRefill { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentTransactionId { get; set; }
        public bool IsSignuped { get; set; }
        public string EciFlag { get; set; }

        public string Cavv { get; set; }

        public string Xid { get; set; }

        public string CVV2Response { get; set; }

        public string AVSResponse { get; set; }

        public string TransactionType { get; set; }

        public string ErrorMessage { get; set; }

        public string ReviewOrderBanner { get; set; }
    }
}