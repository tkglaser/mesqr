using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesqr.Models;

namespace mesqr.API.Models
{
    public class BaseTable
    {
        public BaseTable() { }

        public BaseTable(Table t)
        {
            ID = t.RowGuid;
            Name = t.Name;
            Latitude = t.Latitude;
            Longitude = t.Longitude;
            TableRadius = t.TableRadius;
            Entered = t.Entered;
        }

        public Guid ID { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double TableRadius { get; set; }
        public DateTime Entered { get; set; }
    }
}