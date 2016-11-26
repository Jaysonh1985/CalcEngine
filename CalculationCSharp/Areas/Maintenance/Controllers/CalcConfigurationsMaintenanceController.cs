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
    public class CalcConfigurationsMaintenanceController : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: Maintenance/CalcConfigurationsMaintenance
        public ActionResult Index()
        {
            return View(db.CalcConfiguration.ToList());
        }

        // GET: Maintenance/CalcConfigurationsMaintenance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);
            if (calcConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(calcConfiguration);
        }

        // GET: Maintenance/CalcConfigurationsMaintenance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maintenance/CalcConfigurationsMaintenance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Scheme,Name,User,Configuration,UpdateDate,Version")] CalcConfiguration calcConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.CalcConfiguration.Add(calcConfiguration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(calcConfiguration);
        }

        // GET: Maintenance/CalcConfigurationsMaintenance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);
            if (calcConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(calcConfiguration);
        }

        // POST: Maintenance/CalcConfigurationsMaintenance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Scheme,Name,User,Configuration,UpdateDate,Version")] CalcConfiguration calcConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calcConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(calcConfiguration);
        }

        // GET: Maintenance/CalcConfigurationsMaintenance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);
            if (calcConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(calcConfiguration);
        }

        // POST: Maintenance/CalcConfigurationsMaintenance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);
            db.CalcConfiguration.Remove(calcConfiguration);
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
