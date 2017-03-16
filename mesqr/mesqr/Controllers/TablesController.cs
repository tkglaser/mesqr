using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesqr.Models;
using mesqr.API.Controllers;
using mesqr.API.Models;

namespace mesqr.Controllers
{
    public class TablesController : Controller
    {
        private TableController uc = new TableController();

        //
        // GET: /Tables/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Tables/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(BaseTable table)
        {
            if (ModelState.IsValid)
            {
                uc.PostTable(table);
                return RedirectToAction("Index", "Home");
            }

            return View(table);
        }

    }
}