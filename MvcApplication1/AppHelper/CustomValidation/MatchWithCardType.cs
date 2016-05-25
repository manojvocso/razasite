using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foolproof;

namespace MvcApplication1.AppHelper.CustomValidation
{

    public class MatchWithCardTypeAttribute : ModelAwareValidationAttribute
    {
        //this is needed to register this attribute with foolproof's validator adapter
        public string DependentProperty { get; private set; }

        static MatchWithCardTypeAttribute() { Register.Attribute(typeof(MatchWithCardTypeAttribute)); }

        public MatchWithCardTypeAttribute(string dependentProperty)
        {
            DependentProperty = dependentProperty;
        }


        private object GetDependentPropertyValue(object container)
        {
            var currentType = container.GetType();
            var value = container;

            foreach (string propertyName in DependentProperty.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }

            return value;
        }

        private bool CheckPaymenttypeforPaypal(object container)
        {
            var currentType = container.GetType();
            var value = container;


            var property = currentType.GetProperty("PaymentType");
            value = property.GetValue(value, null);
            //currentType = property.PropertyType;
            return (string)value == PaymentSettings.PaymentType.PayPal.ToString();

        }


        public override bool IsValid(object value, object container)
        {
            //var ispaypal = CheckPaymenttypeforPaypal(container);
            //if (ispaypal)
            //    return true;

            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                //var model = (HomeModel)container;
                return PaymentSettings.GetCardType(value.ToString()).ToLower() ==
                       GetDependentPropertyValue(container).ToString().ToLower();
            }

            //the user wasn't in a constrained role, so just return true
            return true;
        }



    }
}
