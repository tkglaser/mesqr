using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mesqr.Models
{
    public class Table
    {
        public int TableId { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double TableRadius { get; set; }

        #region Owner

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        #endregion


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Entered { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid RowGuid { get; set; }

        public virtual ICollection<Msq> Msqs { get; set; }
    }
}