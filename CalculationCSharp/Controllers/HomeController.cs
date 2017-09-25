// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalculationCSharp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["H1"] = "Project A.I.M";
            ViewData["P1"] = "Providing a Web based calculation engine.";
            return View();
        }

        public ActionResult About()
        {
            ViewData["H1"] = "About";
            ViewData["P1"] = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewData["H1"] = "Contact";
            ViewData["P1"] = "Your contact page.";
            return View();
        }
        public ActionResult Pricing()
        {
            ViewData["H1"] = "Pricing";
            ViewData["P1"] = "Your contact page.";
            return View();
        }
        public ActionResult Privacy()
        {
            ViewData["H1"] = "Privacy";
            return View();
        }
    }
}