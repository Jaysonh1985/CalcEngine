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

namespace CalculationCSharp.Areas.Fire2006.Controllers

{
    public class DeferredController : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: Fire2006/Deferred
        [HttpGet()]
        public ActionResult Input()
        {
             Deferred InputForm = new Deferred();

            InputForm.CalcReference = "1";
            InputForm.DOL = Convert.ToDateTime("17/07/2015");
            InputForm.APP = 23000;
            InputForm.CPD = 1200;
            InputForm.PIDateOverride = Convert.ToDateTime("30/04/2013");
            InputForm.DOB = Convert.ToDateTime("30/09/1973");
            InputForm.MarStat = "Married";
            InputForm.DJS = Convert.ToDateTime("03/04/1991");
            InputForm.Grade = "Firefighter";

            return View("Input", InputForm);
        }

        //POST: Calculation
        [HttpPost()]
        public ActionResult Input(Deferred InputForm)
        {
            DeferredFunctions List = new DeferredFunctions();

            List.Setup(InputForm);

            Session["Output"] = List.List;

            CalculationBase RunCalculation = new CalculationBase();

            RunCalculation.CalculationRun(InputForm, List);
      

           return PartialView("_Output", List.List);

        }

        //// POST: CalculationResults/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateCalcResult([Bind(Include = "Id,User,Scheme,Type,RunDate,Reference,Input,Output")] CalculationResult calculationResult)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CalculationResult.Add(calculationResult);
        //        db.SaveChanges();
        //        return RedirectToAction("Input");
        //    }

        //    return View(calculationResult);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateCalcRegression([Bind(Include = "Id,Scheme,Type,OriginalRunDate,LatestRunDate,Reference,Input,OutputOld,OutputNew,Difference,Pass")] CalculationRegression calculationRegression)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CalculationRegression.Add(calculationRegression);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(calculationRegression);
        //}

        //// POST: CalculationRegressions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditCalcRegression([Bind(Include = "Id,Scheme,Type,OriginalRunDate,LatestRunDate,Reference,Input,OutputOld,OutputNew,Difference,Pass")] CalculationRegression calculationRegression)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(calculationRegression).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(calculationRegression);
        //}

        public ActionResult Download()
        {
            List<OutputList> Output = new List<OutputList>();
            Output = (List<OutputList>)Session["Output"];

            if (Session["Output"] != null)
            {
                return new DownloadFileActionResult(Output, "Output.xls");
            }
            else
            {
                //Some kind of a result that will indicate that the view has 
                //not been created yet. I would use a Javascript message to do so. 

                return new DownloadFileActionResult(Output, "Output.xls"); ;
            }
        }

        public ActionResult Upload(FormCollection formCollection)

        {

            if (Request != null)

            {

                String OutputString = "";
                StringFunctions StringFunctions = new StringFunctions();

                HttpPostedFileBase file = Request.Files["UploadedFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))

                {

                    string fileName = file.FileName;

                    string fileContentType = file.ContentType;

                    byte[] fileBytes = new byte[file.ContentLength];

                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    var usersList = new List<Deferred>();

                    using (var package = new ExcelPackage(file.InputStream))

                    {

                        var currentSheet = package.Workbook.Worksheets;

                        var workSheet = currentSheet.First();

                        var noOfCol = workSheet.Dimension.End.Column;

                        var noOfRow = workSheet.Dimension.End.Row;



                        for (int colIterator = 2; colIterator <= noOfCol; colIterator++)

                        {

                            var model = new Deferred();
                            var properties = new Dictionary<string, object>();
                            var i = 1;
                            
                            foreach (var prop in model.GetType().GetProperties())
                            {

                                var Value = workSheet.Cells[i, colIterator].Value == null ? string.Empty : workSheet.Cells[i, colIterator].Value.ToString();

                                DateTime dateTime;
                                double number;

                                if (DateTime.TryParse(Value,out dateTime))
                                {
                                    
                                    prop.SetValue(model, dateTime, null);

                                }
                                else if (Double.TryParse(Value, out number))
                                {
                                    prop.SetValue(model, number, null);
                                }
                                else if (Value == "")
                                {
                                    prop.SetValue(model, null, null);
                                }
                                else
                                {
                                    prop.SetValue(model, Value, null);
                                }
                               

                                i = i + 1;

                            }

                            usersList.Add(model);

                        }

                        var stringBuilder = new StringBuilder();
                        
                        Response.ClearContent();

                        Response.AddHeader("content-disposition", "attachment;filename=Output.csv");

                        Response.ContentType = "text/csv";

                        int HeaderRow = 0;

                        foreach (var Member in usersList)
                        {
                            DeferredFunctions List = new DeferredFunctions();

                            List.Setup(Member);

                            OutputString = StringFunctions.BulkCalc(List.List,HeaderRow,stringBuilder);
                            HeaderRow = 1;
                           
                        }

                        Response.Write(OutputString);
                        Response.End();
                    }

                }

            }

            return View("Index");
        }

    }
}