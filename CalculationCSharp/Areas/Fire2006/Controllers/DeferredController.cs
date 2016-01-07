using System;
using System.Web.Mvc;
using CalculationCSharp.Areas.Fire2006.Models;
using CalculationCSharp.Controllers;
using CalculationCSharp.Models.Calculation;
using System.Collections.Generic;

namespace CalculationCSharp.Areas.Fire2006.Controllers

{
    public class DeferredController : CalculationBaseController
    {
        public Deferred InputForm = new Deferred();
        public DeferredFunctions List = new DeferredFunctions();
        public CalculationCSharp.Models.XMLFunctions.XMLFunctions xmlfunction = new CalculationCSharp.Models.XMLFunctions.XMLFunctions();

        // GET: Fire2006/Deferred
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
            InputForm.Grade = "Firefighter";

            return View("Input", InputForm);
        }

        //POST: Calculation
        [HttpPost()]
        public ActionResult Input(Deferred InputForm)
        {

            List.Setup(InputForm);

            //Session["Output"] = List.List;
            //Session["Input"] = InputForm;
            //Session["List"] = List;

            string InputXML = xmlfunction.XMLStringBuilder(InputForm);
            string OutputXML = xmlfunction.XMLStringBuilder(List.List);

            CalculationRun(InputXML, OutputXML, InputForm.CalcReference);


            return PartialView("_Output", List.List);

        }

        public ActionResult Calculate(Deferred InputForm, string ActionLink)
        {
            List.Setup(InputForm);

            string InputXML = xmlfunction.XMLStringBuilder(InputForm);
            string OutputXML = xmlfunction.XMLStringBuilder(List.List);

            if (ActionLink == "Download")
            {
                List<OutputList> Output = new List<OutputList>();
                Output = List.List;
                DownloadFileActionResult Download = new DownloadFileActionResult(Output, "Output.xls");

                return new DownloadFileActionResult(Output, "Output.xls");
            }
            else if(ActionLink == "Regression")
            {
                Regression(InputXML, OutputXML, InputForm.CalcReference);
                return View("Input");
            }

            else
            {
                return View("Input");
            }

            
        }
    }
}