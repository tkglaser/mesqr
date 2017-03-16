using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mesqr.Models
{
    public class Msq
    {
        public int MsqId { get; set; }

        [MaxLength(256)]
        public string Message { get; set; }

        [MaxLength(250)]
        public string FriendlyPosition { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Accuracy { get; set; }

        public double? Altitude { get; set; }

        public double? AltitudeAccuracy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Entered { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid RowGuid { get; set; }

        #region User

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        #endregion

        #region Table
        
        public int? TableId { get; set; }

        [ForeignKey("TableId")]
        public Table Table { get; set; }

        #endregion

        #region Replies

        public int? ParentMsqId { get; set; }

        [ForeignKey("ParentMsqId")]
        public virtual Msq ParentMsq { get; set; }

        public virtual ICollection<Msq> Replies { get; set; }

        #endregion

        #region Votes

        public int Score
        {
            get
            {
                return UpVotes - DownVotes;
            }
        }

        public int UpVotes
        {
            get
            {
                if (Votes == null)
                    return 0;

                return Votes.Count(v => v.Up);
            }
        }

        public int DownVotes
        {
            get
            {
                if (Votes == null)
                    return 0;

                return Votes.Count(v => !v.Up);
            }
        }

        public virtual ICollection<Vote> Votes { get; set; }

        #endregion
    }
}