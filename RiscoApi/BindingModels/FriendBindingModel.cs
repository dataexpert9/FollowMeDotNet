using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.BindingModels
{
    public class FriendBindingModel
    {

    }

    public class FriendRequestBindingModel
    {
        public int User_Id { get; set; }

        public int Friend_Id { get; set; }
    }
}