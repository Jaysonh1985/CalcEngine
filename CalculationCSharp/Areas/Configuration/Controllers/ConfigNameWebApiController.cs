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

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class ConfigNameWebApiController : ApiController
    {
        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalcHistoriesController CalcHistories = new CalcHistoriesController();
        CalcHistory CalcHistory = new CalcHistory();

        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(int? id)
        {
            var response = Request.CreateResponse();
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);
            if (calcConfiguration == null)
            {
                
                List<CategoryViewModel> json = repo.GetConfig(null);
                response.Content = new StringContent(JsonConvert.SerializeObject(json));
            }
            else
            {
                response.Content = new StringContent(calcConfiguration.Scheme + " " + calcConfiguration.Name); 
            }
            
            return response;
        }
     }
}
