using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using mesqr.Models;
using mesqr.Models.View;

namespace mesqr.UseCases
{
    public class MsqUseCases : UseCases
    {
        public MsqUseCases(MesqrDb db) : base(db) {}
    
        public IEnumerable<Distance<Msq>> GetNearbyMsqs(
            double Latitude, 
            double Longitude, 
            double Accuracy,
            double? Radius)
        {
            return from m in db.Msqs.Include("User")
                   let d = (SqlFunctions.Acos(SqlFunctions.Sin(Math.PI * m.Latitude / 180.0) * SqlFunctions.Sin(Math.PI * Latitude / 180.0) +
                   SqlFunctions.Cos(Math.PI * m.Latitude / 180.0) * SqlFunctions.Cos(Math.PI * Latitude / 180.0) *
                   SqlFunctions.Cos(Math.PI * m.Longitude / 180.0 - Math.PI * Longitude / 180.0)) * EARTH_RADIUS_MTR).Value
                   where !Radius.HasValue || d < Radius.Value + Accuracy
                   orderby m.Entered descending
                   select new Distance<Msq>()
                   {
                       Item = m,
                       DistanceInMeters = d
                   };
        }
    }
}