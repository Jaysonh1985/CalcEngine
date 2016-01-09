using CalculationCSharp.Areas.Fire2006.Models;
using CalculationCSharp.Models;
using CalculationCSharp.Models.Calculation;
using CalculationCSharp.Models.StringFunctions;
using CalculationCSharp.Models.XMLFunctions;
using CalculationCSharp.Controllers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CalculationCSharp.Controllers
{
    public class CalculationBaseController : Controller
    {
        public CalculationDBContext db = new CalculationDBContext();
        public CalculationCSharp.Models.XMLFunctions.XMLFunctions xmlfunction = new CalculationCSharp.Models.XMLFunctions.XMLFunctions();

        public object Calculate(Object Input, string Scheme, string CalcType, string CalcReference, bool Regression)
        {
            Type input = Type.GetType("CalculationCSharp.Areas."+Scheme+".Models."+CalcType);
            Object inputobj = Activator.CreateInstance(input);

            inputobj = Input;

            Type type = Type.GetType("CalculationCSharp.Areas." + Scheme + ".Models."+CalcType+"Functions");
            MethodInfo methodInfo = type.GetMethod("Setup");
            ParameterInfo[] parameters = methodInfo.GetParameters();
            object classInstance = Activator.CreateInstance(type, null);
            object[] args = new object[] { inputobj };
            object magicvalue = methodInfo.Invoke(classInstance, args);

            string InputXML = xmlfunction.XMLStringBuilder(inputobj);
            string OutputXML = xmlfunction.XMLStringBuilder(magicvalue);

            if(Regression == true)
            {
                CalculationRegressionAdd(InputXML, OutputXML, CalcReference, Scheme, CalcType, true);
            } 
            else
            {
                CalculationResult CalcResult = new CalculationResult();

                CalcResult.Scheme = Scheme;

                if (User.Identity.Name == "")
                {
                    CalcResult.User = "Anon";
                }
                else
                {
                    CalcResult.User = User.Identity.Name;
                }
                CalcResult.Type = CalcType;
                CalcResult.RunDate = DateTime.Now;
                CalcResult.Reference = CalcReference;
                CalcResult.Input = InputXML;
                CalcResult.Output = OutputXML;
                CreateCalcResult(CalcResult);
            }        
            return magicvalue;
        }

        public virtual ActionResult Upload(FormCollection formCollection)

        {

            if (Request != null)

            {
                string OutputString = "";
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

                                if (DateTime.TryParse(Value, out dateTime))
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
                            OutputString = StringFunctions.BulkCalc(List.List, HeaderRow, stringBuilder);
                            HeaderRow = 1;
                        }
                        Response.Write(OutputString);
                        Response.End();
                    }
                }
            }

            return View("Index");
        }

        public void CalculationRegressionAdd(string InputXML, string OutputXML, string CalcReference, string Scheme, string CalcType, Boolean Run)
        {
            CalculationRegression CalcRegression = new CalculationRegression();

            string OldOutput = "";
            int resultid = 0;
            using (var context = new CalculationDBContext())
            {
                var Regression = context.CalculationRegression
                .Where(b => b.Reference == CalcReference && b.Scheme == Scheme && b.Type == CalcType)
                .FirstOrDefault();

                if (Regression == null)
                {
                    resultid = 0;
                }
                else
                {
                    OldOutput = Regression.OutputOld;
                    resultid = Regression.Id;
                }

            }

            if (Run == true)
            {
                XMLFunctions XMLFunction = new XMLFunctions();

                string Difference = XMLFunction.MatchXML(OldOutput, OutputXML);
                CalcRegression.OutputNew = OutputXML;
                CalcRegression.LatestRunDate = DateTime.Now;
                CalcRegression.Difference = Difference;

                if (Difference == null)
                {
                    CalcRegression.Pass = "TRUE";
                }
                else
                {
                    CalcRegression.Pass = "FALSE";
                }
            }
            else
            {
                CalcRegression.Scheme = Scheme;
                CalcRegression.Type = CalcType;
                CalcRegression.Input = InputXML;
                CalcRegression.OutputOld = OutputXML;
                CalcRegression.Reference = CalcReference;
                CalcRegression.OriginalRunDate = DateTime.Now;
                CalcRegression.LatestRunDate = DateTime.Now;
            }

            if (resultid > 0)
            {
                CalcRegression.Id = resultid;
                EditCalcRegression(CalcRegression, Run);
            }
            else
            {
                CreateCalcRegression(CalcRegression);
            }
        }

        public void CreateCalcResult([Bind(Include = "Id,User,Scheme,Type,RunDate,Reference,Input,Output")] CalculationResult calculationResult)
        {
            if (ModelState.IsValid)
            {
                db.CalculationResult.Add(calculationResult);
                db.SaveChanges();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateCalcRegression([Bind(Include = "Id,Scheme,Type,OriginalRunDate,LatestRunDate,Reference,Input,OutputOld,OutputNew,Difference,Pass")] CalculationRegression calculationRegression)
        {
            if (ModelState.IsValid)
            {
                db.CalculationRegression.Add(calculationRegression);
                db.SaveChanges();
            }
        }

        // POST: CalculationRegressions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void EditCalcRegression([Bind(Include = "Id,Scheme,Type,OriginalRunDate,LatestRunDate,Reference,Input,OutputOld,OutputNew,Difference,Pass")] CalculationRegression calculationRegression, Boolean Run)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calculationRegression).State = EntityState.Modified;

                if (Run == true)
                {
                    db.Entry(calculationRegression).Property(x => x.Scheme).IsModified = false;
                    db.Entry(calculationRegression).Property(x => x.Type).IsModified = false;
                    db.Entry(calculationRegression).Property(x => x.Input).IsModified = false;
                    db.Entry(calculationRegression).Property(x => x.OutputOld).IsModified = false;
                    db.Entry(calculationRegression).Property(x => x.Reference).IsModified = false;
                    db.Entry(calculationRegression).Property(x => x.OriginalRunDate).IsModified = false;
                }
                else
                {
                    db.Entry(calculationRegression).Property(x => x.OutputNew).IsModified = false;
                    db.Entry(calculationRegression).Property(x => x.LatestRunDate).IsModified = false;
                    db.Entry(calculationRegression).Property(x => x.Difference).IsModified = false;
                    db.Entry(calculationRegression).Property(x => x.Pass).IsModified = false;
                }
                db.SaveChanges();
                
            }
        }
    }
}