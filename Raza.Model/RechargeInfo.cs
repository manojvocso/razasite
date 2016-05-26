using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class RechargeInfo
    {
        public string OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MemberId { get; set; }
        public string UserType { get; set; }
        public int CardId { get; set; }
        public string Pin { get; set; }
        public double Amount { get; set; }
        public double ServiceFee { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string IpAddress { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string CoupanCode { get; set; }
        public string AniNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string IsAutoRefill { get; set; }
        public double AutoRefillAmount { get; set; }

        public string PaymentTransactionId { get; set; }
        public string EciFlag { get; set; }

        public string Cavv { get; set; }
        public string Xid { get; set; }

        public string CVV2Response { get; set; }

        public string AVSResponse { get; set; }

        public bool IsCcProcess { get; set; }
        
        public string PaymentType { get; set; }

        public string PayerId { get; set; }

        public string AuthTransactionId { get; set; }
        public string CentinelTransactionId { get; set; }
        public string CentinelPayLoad { get; set; }
        public string PayResPayLoad { get; set; }
        public string ProcessedBy { get; set; }      
       

    }

    public class CcValidationModel 
    { 
      //acceptorder=Y,doccprocess=Y,centnelbypass=N,avsbypass=N
        public bool AcceptOrder { get; set; }
        public bool DoCcProcess { get; set; }
        public bool CentinelBypass { get; set; }
        public bool AvsByPass { get; set; }
        public bool IsValidPlan { get; set; }
        public string StatusMsg { get; set; }

    }

    public class CcProcessValidations 
    {

        public CcValidationModel Create(string data)
        {
            var model = new CcValidationModel();
            //"status=1|result=Y,Y,N,N"
            if (data.Split('|').Length > 1)
            {
                string res = data.Split('|')[1].Split('=')[1];
                model.AcceptOrder = res.Split(',')[0] == "Y" ? true : false;
                model.DoCcProcess = res.Split(',')[1] == "Y" ? true : false;
                model.CentinelBypass = res.Split(',')[2] == "Y" ? true : false;
                model.AvsByPass = res.Split(',')[3] == "Y" ? true : false;

            }

            return model;

        }
        

    }

   

    
}
