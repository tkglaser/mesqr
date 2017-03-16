using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mesqr.API.Models;
using mesqr.Models;
using mesqr.UseCases;

namespace mesqr.API.Controllers
{
    public class TableController : ApiMesqrController
    {
        TableUseCases uc;

        public TableController()
        {
            uc = new TableUseCases(db);
        }

        public BaseTable GetTable(Guid id)
        {
            Table table = db.Tables.SingleOrDefault(m => m.RowGuid == id);
            if (table == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new BaseTable(table);
        }

        public IEnumerable<BaseTable> GetSubscribedTables()
        {
            if (!User.Identity.IsAuthenticated)
                return Enumerable.Empty<BaseTable>();

            var subscriptions = (from ts in db.TableSubscriptions
                                 where ts.User.UserName == User.Identity.Name
                                 select ts.Table).ToArray()
                                 .Select(t => new BaseTable(t));

            return subscriptions;
        }

        public IEnumerable<BaseTable> GetNearbyTables(double Latitude, double Longitude, double Accuracy)
        {
            return uc.GetNearbyTables(Latitude, Longitude, Accuracy)
                .ToList()
                .Select(m => new BaseTable(m.Item)).AsEnumerable();
        }

        [Authorize]
        public HttpResponseMessage PostTable([FromBody]BaseTable table)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            return PostTable(table, user);
        }

        public HttpResponseMessage PostTable([FromUri]BaseTable table, string key)
        {
            var session = GetSession(key);

            if (session == null)
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            return PostTable(table, session.User);
        }

        public HttpResponseMessage PostTable([FromUri]BaseTable table, User user)
        {
            if (ModelState.IsValid)
            {
                Table t = new Table()
                {
                    Latitude = table.Latitude,
                    Longitude = table.Longitude,
                    Name = table.Name,
                    TableRadius = table.TableRadius,
                    Owner = user
                };
                db.Tables.Add(t);
                db.SaveChanges();

                if (Request == null)
                    return null;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, new BaseTable(t));
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = t.RowGuid }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
