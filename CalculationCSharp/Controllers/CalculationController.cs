using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using System;
using CalculationCSharp.Models.Calculation;
using System.Collections.Generic;

namespace CalculationCSharp.Controllers
{
    

    public class CalculationController : Controller
    {
        public object List;

        [HttpGet]
        public ActionResult Index(string SchemeType, string CalculationType)
        {
            CalculationCSharp.Models.Calculation.Index IndexCalculation = new CalculationCSharp.Models.Calculation.Index();

            IndexCalculation.CalculationType = CalculationType;
            IndexCalculation.SchemeType = SchemeType;


            return RedirectToAction("Input", CalculationType, new { Area = SchemeType });
        }

    }
}