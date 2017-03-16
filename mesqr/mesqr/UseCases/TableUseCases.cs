using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using mesqr.Models;
using mesqr.Models.View;

namespace mesqr.UseCases
{
    public class TableUseCases : UseCases
    {
        public TableUseCases(MesqrDb db) : base(db) { }

        public IEnumerable<Distance<Table>> GetNearbyTables(double Latitude, double Longitude, double Accuracy)
        {
            return from t in db.Tables
                   let d = (SqlFunctions.Acos(SqlFunctions.Sin(Math.PI * t.Latitude / 180.0) * SqlFunctions.Sin(Math.PI * Latitude / 180.0) +
                   SqlFunctions.Cos(Math.PI * t.Latitude / 180.0) * SqlFunctions.Cos(Math.PI * Latitude / 180.0) *
                   SqlFunctions.Cos(Math.PI * t.Longitude / 180.0 - Math.PI * Longitude / 180.0)) * EARTH_RADIUS_MTR).Value
                   where d <= t.TableRadius + Accuracy * 2.5
                   orderby d descending
                   select new Distance<Table>()
                   {
                       Item = t,
                       DistanceInMeters = d
                   };
        }
    }
}