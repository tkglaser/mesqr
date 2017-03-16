using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mesqr.Models
{
    public class TableSubscription
    {
        public int TableSubscriptionId { get; set; }

        #region User

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        #endregion

        #region Table

        public int TableId { get; set; }

        [ForeignKey("TableId")]
        public Table Table { get; set; }

        #endregion
    }
}