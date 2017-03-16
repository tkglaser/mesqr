using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesqr.Models.View
{
    public class Distance<T>
    {
        public T Item { get; set; }
        public double DistanceInMeters { get; set; }

        public string FormattedDistMiles
        {
            get
            {
                var miles = DistanceInMeters * 0.621371192 / 1000;

                return string.Format("{0:0.0} miles", miles);
            }
        }
        public string FormattedDistISO
        {
            get
            {
                if (DistanceInMeters < 1000)
                    return string.Format("{0:0}m", DistanceInMeters);

                return string.Format("{0:0.0}km", DistanceInMeters / 1000);
            }
        }
    }
}