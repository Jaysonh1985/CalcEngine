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
    public class FunctionNameWebApiController : ApiController
    {
        /// <summary>Controller for the configuration builder.
        /// </summary>
        CalculationDBContext db = new CalculationDBContext();
        /// <summary>Gets the relevant configuration and returns this as JSON object, if no configuration is available creates the default template.
        /// <para>id = CalculationID on DB Table</para>
        /// </summary>
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public IQueryable<CalcFunctions> Get(string Scheme, int ID, string Type)
        {
            if(Type == "Scheme")
            {
                if (Scheme != null)
                {
                    return db.CalcFunctions.Where(i => i.Scheme == Scheme);
                }
                else
                {
                    return null;
                }
            }
            else if(Type == "Config")
            {
                if (ID != 0)
                {
                    return db.CalcFunctions.Where(i => i.ID == ID);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
    }
}
