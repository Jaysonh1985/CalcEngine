using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CalculationCSharp.Models.Calculation.Fire2006;
using CalculationCSharp.Models.Calculation.Fire2006.Deferred;
using System.Web.Routing;
using CalculationCSharp.Models.Calculation;
using System.Web.UI.WebControls;

namespace CalculationCSharp.Controllers

{
    public class Fire2006Controller : Controller
    {

        // GET: Calculation
        [HttpGet()]
        public ActionResult Deferred()
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

            return View("Deferred", InputForm);
        }

        //POST: Calculation
        [HttpPost()]
        public ActionResult Deferred(Deferred InputForm)
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

    }
}