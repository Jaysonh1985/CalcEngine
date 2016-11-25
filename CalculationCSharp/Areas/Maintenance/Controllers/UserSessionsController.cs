using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CalculationCSharp.Models;

namespace CalculationCSharp.Areas.Maintenance.Controllers
{
    public class UserSessionsController : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: Maintenance/UserSessions
        public ActionResult Index()
        {
            return View(db.UserSession.ToList());
        }

        // GET: Maintenance/UserSessions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSession userSession = db.UserSession.Find(id);
            if (userSession == null)
            {
                return HttpNotFound();
            }
            return View(userSession);
        }

        // GET: Maintenance/UserSessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maintenance/UserSessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Section,Record,StartTime")] UserSession userSession)
        {
            if (ModelState.IsValid)
            {
                db.UserSession.Add(userSession);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userSession);
        }

        // GET: Maintenance/UserSessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSession userSession = db.UserSession.Find(id);
            if (userSession == null)
            {
                return HttpNotFound();
            }
            return View(userSession);
        }

        // POST: Maintenance/UserSessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Username,Section,Record,StartTime")] UserSession userSession)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userSession).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userSession);
        }

        // GET: Maintenance/UserSessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSession userSession = db.UserSession.Find(id);
            if (userSession == null)
            {
                return HttpNotFound();
            }
            return View(userSession);
        }

        // POST: Maintenance/UserSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserSession userSession = db.UserSession.Find(id);
            db.UserSession.Remove(userSession);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
