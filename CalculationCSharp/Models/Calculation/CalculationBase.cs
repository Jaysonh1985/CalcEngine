using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CalculationCSharp.Areas.Fire2006.Models;
using CalculationCSharp.Models.Calculation;
using OfficeOpenXml;
using System.Text;
using CalculationCSharp.Models;
using CalculationCSharp.Models.XMLFunctions;
using CalculationCSharp.Models.StringFunctions;
using System.Xml;
using System.Data.Entity;

namespace CalculationCSharp.Models.Calculation
{
   
    public class CalculationBase : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();

        public void CalculationRun(Deferred InputForm, DeferredFunctions List)
        {
            CalculationResult CalcResult = new CalculationResult();

            CalculationCSharp.Models.XMLFunctions.XMLFunctions xmlfunction = new CalculationCSharp.Models.XMLFunctions.XMLFunctions();

            string InputXML = xmlfunction.XMLStringBuilder(InputForm);
            string OutputXML = xmlfunction.XMLStringBuilder(List.List);

            //CalculationRegression calculationRegression = db.CalculationRegression.Find();
            CalculationRegression CalcRegression = new CalculationRegression();

            int resultid = 0;
            using (var context = new CalculationDBContext())
            {
                var Regression = context.CalculationRegression
                .Where(b => b.Reference == InputForm.CalcReference && b.Scheme == "Fire2006" && b.Type == "Deferred")
                .FirstOrDefault();

                #pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (Regression == null)
                {
                    resultid = 0;
                }
                else
                {
                    resultid = Regression.Id;
                }

            }

            //string InputRegXML = calculationRegression.Input.ToString();
            //string OutputOldRegXML = calculationRegression.OutputOld.ToString();
            //string OutputNewRegXML = calculationRegression.OutputNew.ToString();

            string sqlinput = xmlfunction.MatchXML(OutputXML, OutputXML);

            CalcRegression.LatestRunDate = DateTime.Now;
            CalcRegression.Input = InputXML;
            CalcRegression.OutputOld = OutputXML;
            CalcRegression.OutputNew = OutputXML;
            CalcRegression.Scheme = "Fire2006";
            CalcRegression.Type = "Deferred";
            CalcRegression.Reference = InputForm.CalcReference;
            CalcRegression.OriginalRunDate = DateTime.Parse("2015-01-20");
            CalcRegression.Difference = sqlinput;

            if (sqlinput == null)
            {
                CalcRegression.Pass = "TRUE";
            }
            else
            {
                CalcRegression.Pass = "FALSE";
            }

            CalcResult.Scheme = "Fire2006";
            CalcResult.User = "Jayson Herbert";
            CalcResult.Type = "Deferred";
            CalcResult.RunDate = DateTime.Now;
            CalcResult.Reference = InputForm.CalcReference;
            CalcResult.Input = InputXML;
            CalcResult.Output = OutputXML;

            if (resultid > 0)
            {
                CalcRegression.Id = resultid;
                EditCalcRegression(CalcRegression);
            }
            else
            {
                CreateCalcRegression(CalcRegression);
            }

            CreateCalcResult(CalcResult);
        }

        public ActionResult CreateCalcResult([Bind(Include = "Id,User,Scheme,Type,RunDate,Reference,Input,Output")] CalculationResult calculationResult)
        {
            if (ModelState.IsValid)
            {
                db.CalculationResult.Add(calculationResult);
                db.SaveChanges();
                return RedirectToAction("Input");
            }

            return View(calculationResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCalcRegression([Bind(Include = "Id,Scheme,Type,OriginalRunDate,LatestRunDate,Reference,Input,OutputOld,OutputNew,Difference,Pass")] CalculationRegression calculationRegression)
        {
            if (ModelState.IsValid)
            {
                db.CalculationRegression.Add(calculationRegression);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(calculationRegression);
        }

        // POST: CalculationRegressions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCalcRegression([Bind(Include = "Id,Scheme,Type,OriginalRunDate,LatestRunDate,Reference,Input,OutputOld,OutputNew,Difference,Pass")] CalculationRegression calculationRegression)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calculationRegression).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(calculationRegression);
        }


    }
}