using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.AppHelper
{
    public static class AppMessage
    {
        public const string SuccessBillingInfoUpdate = "Your billing information updated successfully";

        public const string SuccessAutorefillActivation = "Auto Refill Successfully Activated on your plan";
        public const string FailureAutoRefillActivation = "Auto Refill Activation fail on your plan";

        public const string SuccessAutorefillDeactivation = "Auto Refill Successfully Deactivated on your plan";
        public const string FailureAutoRefillDeactivation = "Auto Refill Deactivation fail on your plan";

        public const string SuccessAddNewPinlessNumber = "Your pinless number successfully added";
        public const string SuccessDeletePinlessNumber = "Your pinless number successfully deleted";
        public const string FailureDeletePinlessNumber = "Unable to delete your pinlessnumber, Please Try Again";

        public const string SuccessNewOneTouchSetup = "Onetouch setup is added successfully";
        public const string SuccessOneTouchSetupDelete = "Onetouch setup is deleted successfully";
        public const string FailureOneTouchSetupDelete = "Unable to delete your oneTouch setup, Please Try Again";

        public const string SuccessQuickKeysDelete = "Quickkey(s) setup is deleted successfully";
        public const string FailureQuickKeysDelete = "Unable to delete your Quickkey(s) setup, Please Try Again";

        public const string PaymentInfoSuccessfullyUpdated = "Your payment information is successfully updated.";

        public const string SuccessCallForwardDelete = "Your call forwarding number deleted successfully.";
        public const string ErrorCallForwardDelete = "Your call forwarding number doesn't deleted successfully.";

        public const string ErrorInvalidCouponCode = "Invalid Coupon code.";

        public const string SuccessSetupCallForward = "Your Call Forwarding setup is successfully added.";
        public const string ErrorSetupCallForward = "Your Call Forwarding setup cannot be added due to pre assigned date conflict.";
        public const string ErrorFreeTrialNotValid = "Please note, Free Trail offer is strickly for New customers only. Existing customers please check for deals.";

    }


}