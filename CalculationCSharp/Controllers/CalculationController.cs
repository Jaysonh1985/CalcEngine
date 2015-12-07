using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;
using System.Collections;
using System.Web.UI;
using CalculationCSharp.Models.Calculation;

namespace CalculationCSharp.Controllers
{
    public class CalculationController : Controller
    {
         // GET: Calculation
        [HttpGet()]
        public ActionResult Input(CalculationCSharp.Models.Calculation.Index IndexCalculation)
        {
            
            Calculation InputForm = new Calculation();

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
        public ActionResult Input(CalculationCSharp.Models.Calculation.Calculation InputForm)
        {

            return RedirectToAction("Output", InputForm);

        }

        public ActionResult Output(CalculationCSharp.Models.Calculation.Calculation InputForm)
        {
            CalculationCSharp.Models.Calculation.Calculation List = new CalculationCSharp.Models.Calculation.Calculation();

            List.Setup(InputForm);

            object Data = List.List;

            return View(Data);

        }
        
        [HttpGet]       
        public ActionResult Index(string SchemeType, string CalculationType)
        {
            CalculationCSharp.Models.Calculation.Index IndexCalculation = new CalculationCSharp.Models.Calculation.Index();

            IndexCalculation.CalculationType = CalculationType;
            IndexCalculation.SchemeType = SchemeType;
                          

            return RedirectToAction("Input",IndexCalculation);
        }

        public void ExportClientsListToCSV()
        {
            List.Setup(InputForm);

            StringWriter sw = new StringWriter();

            sw.WriteLine("\"ID\",\"Field\",\"Value\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Excel.csv");
            Response.ContentType = "text/csv";


            foreach (var line in List.List)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"",
                                           line.ID,
                                           line.Field,
                                           line.Value));
            }
            Response.Write(sw.ToString());

            Response.End();

        }


        public void ExportClientsListToExcel()
        {
            List.Setup(InputForm);
            var grid = new System.Web.UI.WebControls.GridView();

            grid.DataSource = from data in List.List
                              select new
                              {
                                  ID = data.ID,
                                  Field = data.Field,
                                  Value = data.Value

                              };

            grid.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Excel.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());

            Response.End();

        }
    }
}