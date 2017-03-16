using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesqr.Helpers
{
    public class GeoHelper
    {
        public class GeoBoundingBox
        {
            public double LatMin { get; set; }
            public double LatMax { get; set; }
            public double LonMin { get; set; }
            public double LonMax { get; set; }
        }

        public static GeoBoundingBox GetGeoBoundingBox(double Latitude, double Longitude, double Distance)
        {
            var loc = GeoLocation.fromDegrees(Latitude, Longitude);

            var box = loc.boundingCoordinates(Distance);

            return new GeoBoundingBox()
            {
                LatMin = box[0].Latitude,
                LonMin = box[0].Longitude,
                LatMax = box[1].Latitude,
                LonMax = box[1].Longitude
            };
        }

        public static double Distance(double Lat1, double Lon1, double Lat2, double Lon2)
        {
            return GeoLocation.fromDegrees(Lat1, Lon1).distanceTo(GeoLocation.fromDegrees(Lat2, Lon2));
        }

        private class GeoLocation
        {
            private double radLat;  // latitude in radians
            private double radLon;  // longitude in radians

            private double degLat;  // latitude in degrees
            private double degLon;  // longitude in degrees

            private static readonly double MIN_LAT = ToRadians(-90d);  // -PI/2
            private static readonly double MAX_LAT = ToRadians(90d);   //  PI/2
            private static readonly double MIN_LON = ToRadians(-180d); // -PI
            private static readonly double MAX_LON = ToRadians(180d);  //  PI

            private static readonly double EARTH_RADIUS_MILES = 3959;

            private static double ToRadians(double degrees)
            {
                return Math.PI * degrees / 180.0;
            }

            private static double ToDegrees(double radians)
            {
                return radians * (180.0 / Math.PI);
            }

            private GeoLocation()
            {
            }

            public static GeoLocation fromDegrees(double latitude, double longitude)
            {
                GeoLocation result = new GeoLocation();
                result.radLat = ToRadians(latitude);
                result.radLon = ToRadians(longitude);
                result.degLat = latitude;
                result.degLon = longitude;
                result.checkBounds();
                return result;
            }

            /**
                * @param latitude the latitude, in radians.
                * @param longitude the longitude, in radians.
                */
            public static GeoLocation fromRadians(double latitude, double longitude)
            {
                GeoLocation result = new GeoLocation();
                result.radLat = latitude;
                result.radLon = longitude;
                result.degLat = ToDegrees(latitude);
                result.degLon = ToDegrees(longitude);
                result.checkBounds();
                return result;
            }

            private void checkBounds()
            {
                if (radLat < MIN_LAT || radLat > MAX_LAT ||
                        radLon < MIN_LON || radLon > MAX_LON)
                    throw new ArgumentException();
            }

            public double Latitude
            {
                get
                {
                    return degLat;
                }
            }

            public double Longitude
            {
                get
                {
                    return degLon;
                }
            }

            public override String ToString()
            {
                return "(" + degLat + "\u00B0, " + degLon + "\u00B0) = (" +
                            radLat + " rad, " + radLon + " rad)";
            }

            public double distanceTo(GeoLocation location)
            {
                return Math.Acos(Math.Sin(radLat) * Math.Sin(location.radLat) +
                        Math.Cos(radLat) * Math.Cos(location.radLat) *
                        Math.Cos(radLon - location.radLon)) * EARTH_RADIUS_MILES;
            }

            public GeoLocation[] boundingCoordinates(double distance)
            {

                if (EARTH_RADIUS_MILES < 0d || distance < 0d)
                    throw new ArgumentException();

                // angular distance in radians on a great circle
                double radDist = distance / EARTH_RADIUS_MILES;

                double minLat = radLat - radDist;
                double maxLat = radLat + radDist;

                double minLon, maxLon;
                if (minLat > MIN_LAT && maxLat < MAX_LAT)
                {
                    double deltaLon = Math.Asin(Math.Sin(radDist) /
                        Math.Cos(radLat));
                    minLon = radLon - deltaLon;
                    if (minLon < MIN_LON) minLon += 2d * Math.PI;
                    maxLon = radLon + deltaLon;
                    if (maxLon > MAX_LON) maxLon -= 2d * Math.PI;
                }
                else
                {
                    // a pole is within the distance
                    minLat = Math.Max(minLat, MIN_LAT);
                    maxLat = Math.Min(maxLat, MAX_LAT);
                    minLon = MIN_LON;
                    maxLon = MAX_LON;
                }

                return new GeoLocation[]{fromRadians(minLat, minLon),
				    fromRadians(maxLat, maxLon)};
            }

        }
    }
}