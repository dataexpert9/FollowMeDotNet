using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Friends
    {
        public int Id { get; set; }

        [ForeignKey("Requester")]
        public int Requester_Id { get; set; }

        public User Requester { get; set; }

        [ForeignKey("Addressee")]
        public int Addressee_Id { get; set; }

        public User Addressee { get; set; }

        public int FriendRequestStatus { get; set; }

        public bool Is_Deleted { get; set; }

    }
}
