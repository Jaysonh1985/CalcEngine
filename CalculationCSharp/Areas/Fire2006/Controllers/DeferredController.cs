using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CalculationCSharp.Areas.Fire2006.Models;
using System.Web.Routing;
using CalculationCSharp.Models.Calculation;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.Text;

namespace CalculationCSharp.Areas.Fire2006.Controllers

{
    public class DeferredController : Controller
    {

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
            Deferred List = new Deferred();

            List.Setup(InputForm);

            object Data = List.List;

            Session["Output"] = List.List;

           return PartialView("_Output", Data);

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

                            model.CalcReference = workSheet.Cells[1, colIterator].Value.ToString();
                            model.DOL = Convert.ToDateTime(workSheet.Cells[2, colIterator].Value.ToString());
                            model.APP = Convert.ToDouble(workSheet.Cells[3, colIterator].Value.ToString());
                            model.CPD = Convert.ToDouble(workSheet.Cells[4, colIterator].Value.ToString());
                            model.PIDateOverride = Convert.ToDateTime(workSheet.Cells[5, colIterator].Value.ToString());
                            model.DOB = Convert.ToDateTime(workSheet.Cells[6, colIterator].Value.ToString());
                            model.MarStat = workSheet.Cells[7, colIterator].Value.ToString();
                            model.DJS = Convert.ToDateTime(workSheet.Cells[8, colIterator].Value.ToString());
                            model.AddedYrsService = double.Parse(workSheet.Cells[9, colIterator].Value.ToString());
                            model.TransInService = Convert.ToDouble(workSheet.Cells[10, colIterator].Value.ToString());
                            model.PartTimeService = Convert.ToDouble(workSheet.Cells[11, colIterator].Value.ToString());
                            model.Breaks = Convert.ToDouble(workSheet.Cells[12, colIterator].Value.ToString());
                            model.Grade = workSheet.Cells[13, colIterator].Value.ToString();
                            model.CVofPensionDebit = Convert.ToDouble(workSheet.Cells[14, colIterator].Value.ToString());
                            model.LSI = Convert.ToDecimal(workSheet.Cells[15, colIterator].Value.ToString());
                            model.SCPDPension = Convert.ToDecimal(workSheet.Cells[16, colIterator].Value.ToString());
                            model.SumAVCCont = Convert.ToDecimal(workSheet.Cells[17, colIterator].Value.ToString());

                            usersList.Add(model);


                        }


                        var stringBuilder = new StringBuilder();

                        Response.ClearContent();

                        Response.AddHeader("content-disposition", "attachment;filename=Output.csv");

                        Response.ContentType = "text/csv";

                        int HeaderRow = 0;

                        foreach (var Member in usersList)
                        {
                            Deferred List = new Deferred();

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