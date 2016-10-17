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
    public class ConfigWebApiController : ApiController
    {

        /// <summary>Controller for the configuration builder.
        /// </summary>

        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalcHistoriesController CalcHistories = new CalcHistoriesController();
        CalcHistory CalcHistory = new CalcHistory();

        /// <summary>Gets the relevant configuration and returns this as JSON object, if no configuration is available creates the default template.
        /// <para>id = CalculationID on DB Table</para>
        /// </summary>
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
                if (calcConfiguration.Configuration == null)
                {
                    HttpContext.Current.Cache.Remove("config");
                    List<CategoryViewModel> json = repo.GetConfig(null);
                    response.Content = new StringContent(JsonConvert.SerializeObject(json));
                }
                else
                {                
                    response.Content = new StringContent(calcConfiguration.Configuration);
                }                
            }          
            return response;
        }

        /// <summary>Saves the configuration from the builder
        /// <para>id = CalculationID on DB Table</para>
        /// <para>config = JSON configuration from builder</para>
        /// </summary>
        [System.Web.Http.HttpPut]
        public HttpResponseMessage SetConfig(int id, JObject config)
        {
            //Deserialise the JSON to C# object
            dynamic json = config;
            //Save configuration to calcConfigurations
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);
            calcConfiguration.Configuration = Convert.ToString(json.data);
            calcConfiguration.User = HttpContext.Current.User.Identity.Name.ToString();
            calcConfiguration.UpdateDate = DateTime.Now;
            calcConfiguration.Version = calcConfiguration.Version + (decimal)0.01;
            db.Entry(calcConfiguration).State = EntityState.Modified;
            db.SaveChanges();
            //Build CalcHistory object
            CalcHistory.CalcID = calcConfiguration.ID;
            CalcHistory.Name = calcConfiguration.Name;
            CalcHistory.Scheme = calcConfiguration.Scheme;
            CalcHistory.Configuration = calcConfiguration.Configuration;
            CalcHistory.Comment = "Test";
            CalcHistory.UpdateDate = DateTime.Now;
            CalcHistory.User = calcConfiguration.User;
            CalcHistory.Version = calcConfiguration.Version;
            //Save calcHistory object
            CalcHistories.PostCalcHistory(CalcHistory);
            //Return the response
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(json.data));            
            return response;
        }
        /// <summary>Runs the calculation from builder
        /// <para>id = CalculationID on DB Table</para>
        /// <para>config = JSON configuration from builder</para>
        /// </summary>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(int id,  JObject config)
        {
            dynamic json = config;
            string jsonString = Convert.ToString(json.data);
            //Deserialize JSON to CategoryViewModel then calculate
            List<CategoryViewModel> jCategory = (List<CategoryViewModel>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<CategoryViewModel>));
            Calculate Calculate = new Calculate();
            jCategory = Calculate.DebugResults(jCategory);
            //Update Cache
            repo.UpdateConfig(jCategory);
            //Return the response
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(jCategory));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
     }
}
