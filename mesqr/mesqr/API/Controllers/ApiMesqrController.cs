using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mesqr.Models;

namespace mesqr.API.Controllers
{
    public abstract class ApiMesqrController : ApiController
    {
        protected MesqrDb db = new MesqrDb();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        protected Session GetSession(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;
            var session = db.Sessions.FirstOrDefault(s => s.Key == key && s.Expiry > DateTime.Now);

            return session;
        }
    }
}
