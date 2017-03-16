using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mesqr.Models;

namespace mesqr.API.Controllers
{
    public class TableSubscriptionController : ApiMesqrController
    {
        public IEnumerable<Guid> GetSubscriptionIds(string key)
        {
            var session = GetSession(key);

            if (session == null)
                return Enumerable.Empty<Guid>();

            return from ts in db.TableSubscriptions
                   where ts.UserId == session.UserId
                   select ts.Table.RowGuid;
        }

        public class PostSubscriptionModel
        {
            public Guid TableId { get; set; }
        }

        [Authorize]
        public HttpResponseMessage PostSubscription([FromBody]PostSubscriptionModel model)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            return PostSubscription(model.TableId, user);
        }

        public HttpResponseMessage PostSubscription(Guid TableId, string key)
        {
            var session = GetSession(key);

            if (session == null)
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            return PostSubscription(TableId, session.User);
        }

        public HttpResponseMessage PostSubscription(Guid TableId, User user)
        {
            var table = db.Tables.FirstOrDefault(t => t.RowGuid == TableId);

            if (table == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            if (!db.TableSubscriptions.Any(ts => ts.TableId == table.TableId && ts.UserId == user.UserId))
            {
                db.TableSubscriptions.Add(new TableSubscription()
                {
                    TableId = table.TableId,
                    User = user
                });
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Authorize]
        public HttpResponseMessage DeleteSubscription([FromUri]PostSubscriptionModel model)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            var sub = db.TableSubscriptions.FirstOrDefault(ts => ts.UserId == user.UserId && ts.Table.RowGuid == model.TableId);

            if (sub == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            db.TableSubscriptions.Remove(sub);

            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
