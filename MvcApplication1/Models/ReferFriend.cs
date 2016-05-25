using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class ReferFriend : BaseRazaViewModel
    {
        public string ReferPoint { get; set; }
        public BillingInfoModel ProfileInformation { get; set; }
        public string RedeemPt { get; set; }
        public string Point { get; set; }
        public  string RazaRewardPt { get; set; }
        public string Country { get; set; }
        public int PointToshow { get; set; }

        public List<ReferAFriendModel> ReferFriends { get; set; }
        public List<RazaRewardPoint> RewardPoints { get; set; }
        public List<RedeemModel> RedeemPoints { get; set; }
    }

 

}