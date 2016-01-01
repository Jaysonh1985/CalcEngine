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

            CalculationResult CalcResult = new CalculationResult();

            XMLFunctions xmlfunction = new XMLFunctions();

            string InputXML = xmlfunction.XMLStringBuilder(InputForm);
            string OutputXML = xmlfunction.XMLStringBuilder(List.List);

            CalcResult.Scheme = "Fire2006";
            CalcResult.User = "Jayson Herbert";
            CalcResult.Type = "Deferred";
            CalcResult.RunDate = DateTime.Now;
            CalcResult.Reference = InputForm.CalcReference;
            CalcResult.Input = InputXML;
            CalcResult.Output = OutputXML;

            Create(CalcResult);        

           return PartialView("_Output", List.List);

        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: CalculationResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User,Scheme,Type,RunDate,Reference,Input,Output")] CalculationResult calculationResult)
        {
            if (ModelState.IsValid)
            {
                db.Calculation.Add(calculationResult);
                db.SaveChanges();
                return RedirectToAction("Input");
            }

            return View(calculationResult);
        }

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

                            if (HeaderRow < 1)
                            {
                                foreach (var output in List.List)
                                {
                                    stringBuilder.Append(output.ID);
                                    stringBuilder.Append(",");
                                }
                                stringBuilder.AppendLine();

                                foreach (var output in List.List)
                                {
                                    stringBuilder.Append(output.Field);
                                    stringBuilder.Append(",");
                                }
                                stringBuilder.AppendLine();

                                HeaderRow = 1;
                            }


                            foreach (var output in List.List)
                            {
                                stringBuilder.Append(output.Value);
                                stringBuilder.Append(",");
                            }
                            stringBuilder.AppendLine();


                        }

                        Response.Write(stringBuilder.ToString());
                        Response.End();
                    }

                }

            }

            return View("Index");
        }

    }
}