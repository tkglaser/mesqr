using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using mesqr.Models;

namespace mesqr.API.Controllers
{
    [RequireHttps]
    public class LoginController : ApiMesqrController
    {
        public HttpResponseMessage PostLogin(string User, string Password)
        {
            var user = db.Users.SingleOrDefault(u => u.UserName == User);

            if (user == null || !user.IsPasswordValid(Password))
                return new HttpResponseMessage(HttpStatusCode.Forbidden);

            var session = (from s in db.Sessions
                          where s.UserId == user.UserId
                          where s.Expiry > DateTime.Now
                          select s).SingleOrDefault();

            if (session == null)
            {
                session = new Session()
                {
                    Expiry = DateTime.Now.AddDays(1),
                    UserId = user.UserId
                };

                do
                {
                    session.Key = Session.RandomString(25);
                } while (db.Sessions.Any(s => s.Key == session.Key));

                db.Sessions.Add(session);
                db.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK, session.Key);
        }
    }
}