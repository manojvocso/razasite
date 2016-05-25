using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcApplication1.Areas.Mobile.Models;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class CheckoutViewModel
    {
        public ShopCartMobileModel ShopCartModel { get; set; }

    }
}