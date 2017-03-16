using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesqr.Models;

namespace mesqr.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private MesqrDb db = new MesqrDb();

        private ActionResult UpOrDown(int id, string r, bool up)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);
            var msq = db.Msqs.Find(id);

            var vote = db.Votes.SingleOrDefault(v => v.UserId == user.UserId && v.MsqId == id);
            if (vote == null)
            {
                vote = new Vote()
                {
                    MsqId = id,
                    UserId = user.UserId
                };

                db.Votes.Add(vote);
            }

            vote.Up = up;

            db.SaveChanges();

            return Redirect(r);
        }

        public ActionResult Up(int id, string r)
        {
            return UpOrDown(id, r, up: true);
        }

        public ActionResult Down(int id, string r)
        {
            return UpOrDown(id, r, up: false);
        }
    }
}
