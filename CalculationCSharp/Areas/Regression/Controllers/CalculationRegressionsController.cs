﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CalculationCSharp.Models;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using CalculationCSharp.Areas.Fire2006.Models;
using CalculationCSharp.Models.Calculation;
using System.Xml.Linq;

namespace CalculationCSharp.Areas.Regression.Controllers
{
    public class CalculationRegressionsController : Controller
    {
        public CalculationDBContext db = new CalculationDBContext();
        public CalculationCSharp.Controllers.CalculationBaseController RunCalculation = new CalculationCSharp.Controllers.CalculationBaseController();
        // GET: Regression/CalculationRegressions
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


            if (type == "Input")
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

            var model = result as Deferred;

            List.Setup(model);

            CalculationCSharp.Models.XMLFunctions.XMLFunctions xmlfunction = new CalculationCSharp.Models.XMLFunctions.XMLFunctions();

            string InputXML = xmlfunction.XMLStringBuilder(model);
            string OutputXML = xmlfunction.XMLStringBuilder(List.List);

            RunCalculation.CalculationRegressionAdd(InputXML, OutputXML, model.CalcReference,calculationRegression.Scheme,calculationRegression.Type, true);

            return RedirectToAction("Index");
        }

        public ActionResult RunAll()
        {
            CalculationRegression calculationRegression = new CalculationRegression();

            var AllCalcs = db.CalculationRegression.ToList();

            foreach (var Calc in AllCalcs)
            {
                var serializer = new XmlSerializer(typeof(Areas.Fire2006.Models.Deferred));
                object result;

                using (TextReader reader = new StringReader(Calc.Input))
                {
                    result = serializer.Deserialize(reader);
                }

                DeferredFunctions List = new DeferredFunctions();

                var model = result as Deferred;

                List.Setup(model);

                CalculationCSharp.Models.XMLFunctions.XMLFunctions xmlfunction = new CalculationCSharp.Models.XMLFunctions.XMLFunctions();

                string InputXML = xmlfunction.XMLStringBuilder(model);
                string OutputXML = xmlfunction.XMLStringBuilder(List.List);

                RunCalculation.CalculationRegressionAdd(InputXML, OutputXML, model.CalcReference, Calc.Scheme, Calc.Type, true);

            }

            return RedirectToAction("Index");
        }

        // GET: Regression/CalculationRegressions/Delete/5
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

        // POST: Regression/CalculationRegressions/Delete/5
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