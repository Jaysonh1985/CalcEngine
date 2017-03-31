// Copyright (c) 2016 Project AIM
using CalculationCSharp.Areas.Configuration.Models;
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using NCalc;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Web;
using CalculationCSharp.Models.Calculation;
using CalculationCSharp.Areas.Configuration.Models.Actions;
using CalculationCSharp.Areas.Configuration.Controllers;
using log4net;
using System.Linq;
using System.IO;
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class FactorTableWebApiController : ApiController
    {
        
        /// <summary>Gets a list of factor tables to choose from
        /// </summary>
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            // Put all file names in root directory into array.
            string[] array1 = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("\\Factor Tables\\"));
            List<SelectListItem> li = new List<SelectListItem>();
            int i = 0;
            // Display all files.
            Console.WriteLine("--- Files: ---");
            foreach (string name in array1)
            {
                string x = Convert.ToString(i);
                var pathParts = name.Split(Path.DirectorySeparatorChar);
                string fileName = pathParts.Last();
                fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                li.Add(new SelectListItem { Text = fileName, Value = fileName });
                i += i;
            }
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(li));  
            return response;
        }
     }
}
