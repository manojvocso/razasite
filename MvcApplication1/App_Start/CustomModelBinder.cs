using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Areas.Mobile.ViewModels;

namespace MvcApplication1.App_Start
{
    public class CustomModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(UpdateCreditCardViewModel))
            {
                var bindModel = (UpdateCreditCardViewModel)base.BindModel(controllerContext, bindingContext);
                if (bindModel != null && !string.IsNullOrEmpty(bindModel.CreditCardNumber))
                    bindModel.CreditCardNumber = bindModel.CreditCardNumber.Replace(" ", "");

                return bindModel;
            }
            else if (bindingContext.ModelType == typeof(CartPaymentInfoViewModel))
            {
                var bindModel = (CartPaymentInfoViewModel)base.BindModel(controllerContext, bindingContext);
                if (bindModel != null && !string.IsNullOrEmpty(bindModel.CreditCardNumber))
                    bindModel.CreditCardNumber = bindModel.CreditCardNumber.Replace(" ", "");

                return bindModel;
            }
            else
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }
    }
}