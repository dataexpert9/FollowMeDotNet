using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static BasketApi.Global;

namespace WebApplication1.BindingModels
{
    public class CreatePostBindingModel
    {
        public CreatePostBindingModel()
        {
            UserTagId = new List<int>();
            CheckIns = new List<PostCheckInBindingModel>();
        }

        public string Text { get; set; }

        [Required]
        public PostVisibilityTypes Visibility { get; set; }

        public string Location { get; set; }

        public string ImageUrls { get; set; }

        public List<string> FeelingActivity { get; set; }
       
        public List<int> UserTagId { get; set; }

        public List<PostCheckInBindingModel> CheckIns { get; set; }


        //public string VideoUrls { get; set; }
    }

    public class PostCheckInBindingModel
    {
        public string LocationName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

}