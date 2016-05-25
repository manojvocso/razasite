using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Areas.Mobile.Models;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class OrderHistoryViewModel : BaseMobileViewModel
    {
        public OrderHistoryViewModel()
        {
            Orders = new List<OrderHistoryEntity>();
        }
        public List<OrderHistoryEntity> Orders { get; set; }
    }
}