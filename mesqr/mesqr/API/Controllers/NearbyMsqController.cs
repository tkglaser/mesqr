using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mesqr.API.Models;
using mesqr.Models;
using mesqr.Models.View;
using mesqr.UseCases;

namespace mesqr.API.Controllers
{
    public class NearbyMsqController : ApiMesqrController
    {
        MsqUseCases uc;

        public NearbyMsqController()
        {
            uc = new MsqUseCases(db);
        }

        public IEnumerable<BaseMsq> GetNearbyMsqs(
            double Latitude, 
            double Longitude, 
            double Accuracy,
            double? Radius,
            int? skip,
            int? take)
        {
            var rawmodel = uc.GetNearbyMsqs(Latitude, Longitude, Accuracy, Radius);
            if (skip.HasValue)
                rawmodel = rawmodel.Skip(skip.Value);

            if (take.HasValue)
                rawmodel = rawmodel.Take(take.Value);

            return rawmodel
                .ToList()
                .Select(m => new BaseMsq(m.Item)).AsEnumerable();
        }
    }
}
