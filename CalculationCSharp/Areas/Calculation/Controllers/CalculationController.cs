// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Calculation.Controllers
{
    public class CalculationController : Controller
    {
        /// <summary>Controller for displaying the calculation release pages
        /// </summary>

        // GET: Calculation/CalcReleasePage
        public ActionResult Index()
        {
            return View();
        }

        // GET: Calculation/CalcReleasePage
        public ActionResult Form()
        {
            return View();
        }

    }
}