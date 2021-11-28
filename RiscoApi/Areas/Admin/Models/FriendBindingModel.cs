using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static BasketApi.Global;

namespace WebApplication1.Areas.Admin.Models
{
    public class FriendBindingModel
    {
        public int User_Id { get; set; }

        public int Friend_Id { get; set; }

    }

    public class AcceptRejectFriendRequestBindingModel
    {
        public int User_Id { get; set; }

        public int Friend_Id { get; set; }

        public FriendRequestStatus Status { get; set; }
    }
}