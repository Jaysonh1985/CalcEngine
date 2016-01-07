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
using System.Xml.Linq;
using System.Text;

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

        public ActionResult ViewXML(int? id, string type)
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

            StringBuilder builder = new StringBuilder();

            XElement xmlTree = null;

            if (type == "OutputOld")
            {
                xmlTree = XElement.Parse(calculationRegression.OutputOld);
            }
            else if (type == "OutputNew")
            {
                xmlTree = XElement.Parse(calculationRegression.OutputNew);
            }
            else if (type == "Input")
            {
                xmlTree = XElement.Parse(calculationRegression.Input);
            }

            else if (type == "Difference")
            {
                xmlTree = XElement.Parse(calculationRegression.Difference);
            }


            if(type=="Input")
            {
                xmlTree = XElement.Parse(calculationRegression.Input);

                IEnumerable<XElement> childElements =
                from el in xmlTree.Elements()
                select el;
                foreach (XElement el in childElements)
                    builder.Append(el.Name).Append(": ").Append(el.Value).AppendLine();
            }
            else
            {
                foreach (XElement element in xmlTree.Descendants("OutputList"))
                {
                    foreach (XElement childEllement in element.Descendants())
                    {
                        builder.Append(childEllement.Value).Append(" | ");
                    }

                    builder.AppendLine();
                }
            }

            RegressionViewModel myViewmodel = new RegressionViewModel();

            myViewmodel.YourString = builder.ToString();

            return PartialView("_RegModal", myViewmodel);
        }

        // GET: CalculationRegressions/Create
        public ActionResult Run(int? id)
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

            RunCalculation.CalculationRegressionAdd(model, List,true);

            return RedirectToAction("Index");
        }

        public ActionResult RunAll()
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

                RunCalculation.CalculationRegressionAdd(model, List, true);

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
