using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mesqr.Models
{
    public class Vote
    {
        public int VoteId { get; set; }

        public bool Up { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int MsqId { get; set; }

        [ForeignKey("MsqId")]
        public virtual Msq Msq { get; set; }
    }
}