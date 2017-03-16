using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesqr.Models;

namespace mesqr.API.Models
{
    public class BaseMsq
    {
        public BaseMsq()
        {
            // Default ctor needed for XML serialisation
        }

        public BaseMsq(Msq msq)
        {
            Message = msq.Message;
            FriendlyPosition = msq.FriendlyPosition;
            Latitude = msq.Latitude;
            Longitude = msq.Longitude;
            Accuracy = msq.Accuracy;
            ID = msq.RowGuid;
            UserName = msq.User.UserName;
            Entered = msq.Entered;

            if (msq.TableId.HasValue)
                TableID = msq.Table.RowGuid;
        }

        public Guid ID { get; set; }
        public string Message { get; set; }
        public string FriendlyPosition { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public string UserName { get; set; }
        public Guid? TableID { get; set; }
        public DateTime Entered { get; set; }
    }
}