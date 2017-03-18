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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
        CalculationCSharp.Models.ApplicationDbContext context = new CalculationCSharp.Models.ApplicationDbContext();

        /// <summary>Gets the relevant configuration and returns this as JSON object, if no configuration is available creates the default template.
        /// <para>id = CalculationID on DB Table</para>
        /// </summary>
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public IQueryable<string> Get()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            string[] myInClause = null;
            var response = Request.CreateResponse();
            if (user.Scheme != null)
            {
                myInClause = user.Scheme.Split(',');
                return db.Schemes.Where(s => myInClause.Contains(s.Name)).Select(m => m.Name).Distinct();
            }
            else
            {
                return null;
            }              
        }    
     }
}
