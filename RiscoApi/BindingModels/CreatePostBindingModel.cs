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
        }

        public string Text { get; set; }

        [Required]
        public PostVisibilityTypes Visibility { get; set; }

        public string Location { get; set; }

        public string ImageUrls { get; set; }

        public List<int> UserTagId { get; set; }


        //public string VideoUrls { get; set; }
    }
}