using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FeelingActivities
    {
        public int Id { get; set; }

        public string FeelingType { get; set; }

        public int Is_Deleted { get; set; }

        [ForeignKey("Post")]
        public int? Post_Id { get; set; }
        public Post Post { get; set; }


    }
}
