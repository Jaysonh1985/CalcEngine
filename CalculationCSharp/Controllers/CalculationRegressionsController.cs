using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CalculationCSharp.Models;
using CalculationCSharp.Models.Calculation;
using System.Xml.Serialization;
using System.IO;
using CalculationCSharp.Areas.Fire2006.Models;

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
        public ActionResult Create(int? id)
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

            var serializer = new XmlSerializer(typeof(Areas.Fire2006.Models.Deferred));
            object result;

            

            using (TextReader reader = new StringReader(calculationRegression.Input))
            {
                result = serializer.Deserialize(reader);
            }

            DeferredFunctions List = new DeferredFunctions();
            CalculationBase RunCalculation = new CalculationBase();

            var model = result as Deferred;

            List.Setup(model);

            RunCalculation.CalculationRun(model,List);
            
            return RedirectToAction("Index");
        }

        public ActionResult CreateAll()
        {
            CalculationRegression calculationRegression = new CalculationRegression();

            var test = db.CalculationRegression.ToList();

            foreach (var Tes in test)
            {
                var serializer = new XmlSerializer(typeof(Areas.Fire2006.Models.Deferred));
                object result;

                using (TextReader reader = new StringReader(Tes.Input))
                {
                    result = serializer.Deserialize(reader);
                }

                DeferredFunctions List = new DeferredFunctions();
                CalculationBase RunCalculation = new CalculationBase();

                var model = result as Deferred;

                List.Setup(model);

                RunCalculation.CalculationRun(model, List);

            } 


            return RedirectToAction("Index");
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
