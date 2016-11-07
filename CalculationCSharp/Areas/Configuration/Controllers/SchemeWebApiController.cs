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
using System.Linq;
using CalculationCSharp.Models.Calculation;
using CalculationCSharp.Areas.Configuration.Models.Actions;
using CalculationCSharp.Areas.Configuration.Controllers;

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class SchemeWebApiController : ApiController
    {

        /// <summary>Controller for the configuration builder.
        /// </summary>

        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        Scheme scheme = new Scheme();

        /// <summary>Gets the relevant configuration and returns this as JSON object, if no configuration is available creates the default template.
        /// <para>id = CalculationID on DB Table</para>
        /// </summary>
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public IQueryable<string> Get()
        {
            var response = Request.CreateResponse();

            var result = db.Schemes.Select(m => m.Name).Distinct();
      
            return result;
        }
    
     }
}
