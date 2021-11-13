using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ResetPasswordCode
    {
        public int Id { get; set; }

        public string Code { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }

        public User User { get; set; }

        public bool IsExpired { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }


    }
}
