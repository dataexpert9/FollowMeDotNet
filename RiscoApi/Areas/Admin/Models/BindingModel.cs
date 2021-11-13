using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace BasketApi.Areas.SubAdmin.Models
{
    public class AddAdminBindingModel
    {
        [Required]
        public string StoreName { get; set; }

        public decimal Long { get; set; }

        public decimal Lat { get; set; }

        public short Description { get; set; }
    }

}