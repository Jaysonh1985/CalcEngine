using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CalculationCSharp.Models;

namespace CalculationCSharp.Controllers
{
    public class CalculationRegressionsController : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: CalculationRegressions
        public ActionResult Index()
        {
            return View(db.CalculationRegression.ToList());
        }

        public ActionResult ViewXML(int? id)
        {
            CalculationRegression calculationRegression = db.CalculationRegression.Find(id);

            return PartialView("_RegModal",calculationRegression);
        }

        //[HttpPost]
        //public ActionResult AddNote(CalculationRegression model)
        //{
        //    return RedirectToAction("Index");
        //}

        // GET: CalculationRegressions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalculationRegression calculationRegression = db.CalculationRegression.Find(id);
            if (calculationRegression == null)
            {
                return HttpNotFound();
            }
            return View(calculationRegression);
        }

        // GET: CalculationRegressions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CalculationRegressions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Scheme,Type,OriginalRunDate,LatestRunDate,Reference,Input,OutputOld,OutputNew,Difference,Pass")] CalculationRegression calculationRegression)
        {
            if (ModelState.IsValid)
            {
                db.CalculationRegression.Add(calculationRegression);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(calculationRegression);
        }

        // GET: CalculationRegressions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalculationRegression calculationRegression = db.CalculationRegression.Find(id);
            if (calculationRegression == null)
            {
                return HttpNotFound();
            }
            return View(calculationRegression);
        }

        // POST: CalculationRegressions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Scheme,Type,OriginalRunDate,LatestRunDate,Reference,Input,OutputOld,OutputNew,Difference,Pass")] CalculationRegression calculationRegression)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calculationRegression).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(calculationRegression);
        }

        // GET: CalculationRegressions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalculationRegression calculationRegression = db.CalculationRegression.Find(id);
            if (calculationRegression == null)
            {
                return HttpNotFound();
            }
            return View(calculationRegression);
        }

        // POST: CalculationRegressions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalculationRegression calculationRegression = db.CalculationRegression.Find(id);
            db.CalculationRegression.Remove(calculationRegression);
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
