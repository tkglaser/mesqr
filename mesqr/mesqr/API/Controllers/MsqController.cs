using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using mesqr.API.Models;
using mesqr.Models;

namespace mesqr.API.Controllers
{
    public class MsqController : ApiMesqrController
    {
        // GET api/Msq
        public IEnumerable<BaseMsq> GetMsqs()
        {
            //ObjectContent<IEnumerable<BaseMsq>> responseContent = new ObjectContent<IEnumerable<BaseMsq>>(
            //    db.Msqs.Include("User").ToList().Select(m => new BaseMsq(m)).ToArray().AsEnumerable(),
            //    new XmlMediaTypeFormatter()); // change the formatters accordingly

            //MemoryStream ms = new MemoryStream();

            //// This line would cause the formatter's WriteToStream method to be invoked.
            //// Any exceptions during WriteToStream would be thrown as part of this call
            //responseContent.CopyToAsync(ms).Wait();

            var msqs = db.Msqs.Include("User");
            return msqs.ToList().Select(m => new BaseMsq(m))
                .AsEnumerable();
        }

        // GET api/Msq/5
        public BaseMsq GetMsq(Guid id)
        {
            Msq msq = db.Msqs.SingleOrDefault(m => m.RowGuid == id);
            if (msq == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new BaseMsq(msq);
        }

        // PUT api/Msq/5
        public HttpResponseMessage PutMsq(int id, Msq msq)
        {
            if (ModelState.IsValid && id == msq.MsqId)
            {
                db.Entry(msq).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, msq);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [Authorize]
        public HttpResponseMessage PostMsq([FromBody]BaseMsq msq)
        {
            var user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            return PostMsq(msq, user);
        }

        public HttpResponseMessage PostMsq([FromUri]BaseMsq msq, string key)
        {
            var session = GetSession(key);

            if (session == null)
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            return PostMsq(msq, session.User);
        }

        private HttpResponseMessage PostMsq(BaseMsq msq, User user)
        {
            if (ModelState.IsValid)
            {
                var m = new Msq()
                {
                    Accuracy = msq.Accuracy,
                    FriendlyPosition = msq.FriendlyPosition,
                    Latitude = msq.Latitude,
                    Longitude = msq.Longitude,
                    Message = msq.Message,
                    User = user
                };
                if (msq.TableID.HasValue)
                {
                    m.TableId = db.Tables.Single(t => t.RowGuid == msq.TableID.Value).TableId;
                }
                db.Msqs.Add(m);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, new BaseMsq(m));
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = m.RowGuid }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Msq/5
        public HttpResponseMessage DeleteMsq(int id)
        {
            Msq msq = db.Msqs.Find(id);
            if (msq == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Msqs.Remove(msq);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, msq);
        }
    }
}