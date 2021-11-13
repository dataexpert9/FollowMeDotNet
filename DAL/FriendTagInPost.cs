using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FriendTagInPost
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }

        public User User { get; set; }

        [ForeignKey("Post")]
        public int Post_Id { get; set; }

        public Post Post { get; set; }

        public bool IsDeleted { get; set; }

    }
}
