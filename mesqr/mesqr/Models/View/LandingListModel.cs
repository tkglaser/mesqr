using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesqr.Models.View
{
    public class LandingListModel
    {
        public IEnumerable<Distance<Msq>> NearbyMsqs { get; set; }
        public IEnumerable<Distance<Table>> NearbyTables { get; set; }
    }
}