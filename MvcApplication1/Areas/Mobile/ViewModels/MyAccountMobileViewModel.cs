using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Areas.Mobile.Models;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class MyAccountMobileViewModel : BaseMobileViewModel
    {
        public MyAccountMobileViewModel()
        {
            CustomerPlanList = new List<MyAccountPlanEntity>();
        }

        public List<MyAccountPlanEntity> CustomerPlanList { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }



}