using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CheckIn
    {
        public int Id { get; set; }

        public string LocationName { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [ForeignKey("Post")]
        public int? Post_Id { get; set; }

        public Post Post { get; set; }
    }
}
