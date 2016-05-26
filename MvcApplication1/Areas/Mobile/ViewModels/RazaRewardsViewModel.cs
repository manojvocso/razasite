using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class RazaRewardsViewModel : BaseMobileViewModel
    {
        public int TotalEarnedPointsByRecharge { get; set; }
        public int ReferAFriend { get; set; }
        public int PointsRedeemed { get; set; }
        public int TotalAvailablePoints { get; set; }
    }
}