using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoogleMapsApi;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using mesqr.Models;
using mesqr.Models.View;
using mesqr.UseCases;

namespace mesqr.Controllers
{
    public class HomeController : Controller
    {
        private MesqrDb db;
        private MsqUseCases ucMsq;
        private TableUseCases ucTable;

        public HomeController()
        {
            db = new MesqrDb();
            ucMsq = new MsqUseCases(db);
            ucTable = new TableUseCases(db);
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult List(double Latitude, double Longitude, double Accuracy)
        {
            var model = new LandingListModel();
            model.NearbyMsqs = ucMsq.GetNearbyMsqs(Latitude, Longitude, Accuracy, null).ToList();

            model.NearbyTables = ucTable.GetNearbyTables(Latitude, Longitude, Accuracy);
            return View(model);
        }

        public ActionResult GetFriendlyName(double lat, double lon)
        {
            var result = GoogleMaps.Geocode.Query(new GeocodingRequest()
            {
                Location = new Location(lat, lon)
            });
            if (result.Results.Any())
            {
                var fr = result.Results.First();
                return Content(fr.FormattedAddress);
            }
            return Content("");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}