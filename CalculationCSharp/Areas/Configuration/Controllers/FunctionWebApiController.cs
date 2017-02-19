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

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class FunctionWebApiController : ApiController
    {

        /// <summary>Controller for the configuration builder.
        /// </summary>

        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalcHistoriesController CalcHistories = new CalcHistoriesController();
        CalcHistory CalcHistory = new CalcHistory();
        private static readonly ILog logger = LogManager.GetLogger("Name");

        /// <summary>Gets the relevant configuration and returns this as JSON object, if no configuration is available creates the default template.
        /// <para>id = CalculationID on DB Table</para>
        /// </summary>
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(int? id)
        {
            var response = Request.CreateResponse();
            CalcFunctions calcFunction = db.CalcFunctions.Find(id);
            if (calcFunction == null)
            {            
                List<CategoryViewModel> json = repo.GetConfig(null);
                response.Content = new StringContent(JsonConvert.SerializeObject(json));
            }
            else
            {
                if (calcFunction.Configuration == null)
                {
                    HttpContext.Current.Cache.Remove("config");
                    List<CategoryViewModel> json = repo.GetConfig(null);
                    response.Content = new StringContent(JsonConvert.SerializeObject(json));
                }
                else
                {                
                    response.Content = new StringContent(calcFunction.Configuration);
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
            CalcFunctions calcFunction = db.CalcFunctions.Find(id);
            calcFunction.Configuration = Convert.ToString(json.data);
            calcFunction.User = HttpContext.Current.User.Identity.Name.ToString();
            calcFunction.UpdateDate = DateTime.Now;
            calcFunction.Version = calcFunction.Version + (decimal)0.001;
            db.Entry(calcFunction).State = EntityState.Modified;
            db.SaveChanges();
            ////Build CalcHistory object
            //CalcHistory.CalcID = calcFunction.ID;
            //CalcHistory.Name = calcFunction.Name;
            //CalcHistory.Scheme = calcFunction.Scheme;
            //CalcHistory.Configuration = calcFunction.Configuration;
            //CalcHistory.Comment = Convert.ToString(json.comment);
            //CalcHistory.UpdateDate = DateTime.Now;
            //CalcHistory.User = calcFunction.User;
            //CalcHistory.Version = calcFunction.Version;
            ////Save calcHistory object
            //CalcHistories.PostCalcHistory(CalcHistory);
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
            logger.Debug("End - " + HttpContext.Current.User.Identity.Name.ToString());
            //Return the response
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(jCategory));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
     }
}
