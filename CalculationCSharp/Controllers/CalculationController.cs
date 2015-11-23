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

namespace Engine.Controllers
{
    public class CalculationController : Controller
    {
        public static CalculationCSharp.Models.Calculation.Calculation InputForm = new CalculationCSharp.Models.Calculation.Calculation();

        public CalculationCSharp.Models.Calculation.Calculation List = new CalculationCSharp.Models.Calculation.Calculation();

        // GET: Calculation
        [HttpGet()]
        public ActionResult Input()
        {

            InputForm.CalcReference = "1";
            InputForm.DOL = Convert.ToDateTime("17/07/2015");
            InputForm.APP = 23000;
            InputForm.CPD = 1200;
            InputForm.PIDateOverride = Convert.ToDateTime("30/04/2013");
            InputForm.DOB = Convert.ToDateTime("30/09/1973");
            InputForm.MarStat = "Married";
            InputForm.DJS = Convert.ToDateTime("03/04/1991");
            //AddedYrsService()
            //TransInService()
            //PartTimeService()
            //Breaks()
            InputForm.Grade = "Firefighter";
            //CVofPensionDebit()
            //LSI()
            //SCPDPension()
            //SumAVCCont()

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

            List.Setup(InputForm);

            object Data = List.List;

            //object dataarray = Data.ToArray;

            //CSVOutput(Data);

            return View(Data);

        }


        //void CSVOutput(object data)
        //{
        //	StringWriter sw = new StringWriter();

        //	sw.WriteLine("\"ID\",\"Field\",\"Value\",\"Group\"");

        //	Response.ClearContent();
        //	Response.AddHeader("content-disposition", "attachment;filename=Members.csv");
        //	Response.ContentType = "text/csv";

        //	foreach (object line in data) {
        //		sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", line.id, line.field, line.value, line.@group));
        //	}

        //	Response.Write(sw.ToString());

        //	Response.End();
        //}
    }
}