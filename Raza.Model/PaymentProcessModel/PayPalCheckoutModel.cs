using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model.PaymentProcessModel
{
    public class PayPalCheckoutModel
    {
        /// <summary>
        /// Guideline for processing Payment 
        /// Get details for check is centinel process
        /// </summary>
        public PaymentProcessGuide PaymentProcessGuide { get; set; }

        /// <summary>
        /// Plan information for recharge or new plan purchase checkout
        /// </summary>
        public ProcessPlanInfo ProcessPlanInfo { get; set; }

        /// <summary>
        /// User infor who processing the payment
        /// </summary>
        public BillingInfo ProcessByUserInfo { get; set; }

        /// <summary>
        /// Payment information for processing payment
        /// </summary>
        public ProcessPaymentInfo ProcessPaymentInfo { get; set; }

        /// <summary>
        /// New Generated Orderid
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Final Status of transaction
        /// </summary>
        public bool FinalTransactionStatus { get; set; }

        /// <summary>
        /// to handle review order order message
        /// </summary>
        public string FinalResponseMessage { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public ViewMessage Message { get; set; }

        /// <summary>
        /// Centinel Authentication response , set in paymentccverifier...
        /// </summary>
        public CentinelAuthenticationResponse CentinelAuthenticationResponse { get; set; }

        /// <summary>
        /// Set values at time of authorization in cardlookup
        /// </summary>
        public CentinelAutorizationResponse CentinelAutorizationResponse { get; set; }

    }
}
