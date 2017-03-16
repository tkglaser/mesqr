using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesqr.Models;

namespace mesqr.UseCases
{
    public class UseCases
    {
        protected MesqrDb db;
        protected static readonly int EARTH_RADIUS_MTR = 6371392;

        protected UseCases(MesqrDb db)
        {
            this.db = db;
        }
    }
}