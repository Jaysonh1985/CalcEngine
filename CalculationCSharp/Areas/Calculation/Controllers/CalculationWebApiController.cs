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

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class CalculationWebApiController : ApiController
    {
        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        /// <summary>Controller for getting the configuration for the calculation screen and running the calculations.
        /// </summary>

        /// <summary>Get configuration, if calculation == null then post template.
        /// <para>id = CalculationID on DB Table </para>
        /// </summary>

        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(int? id)
        {
            var response = Request.CreateResponse();
            CalcRelease calcConfiguration = db.CalcRelease.Find(id);
            if (calcConfiguration == null)
            {
                List<CategoryViewModel> json = repo.GetConfig(null);
                response.Content = new StringContent(JsonConvert.SerializeObject(json));
            }
            else
            {
                if (calcConfiguration.Configuration == null)
                {
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

        /// <summary>Update config for calculation from Configuration Menu.
        /// <para>id = CalculationID on DB Table</para>
        /// <para>config = JSON Config from configutation App</para>
        /// </summary>

        [System.Web.Http.HttpPut]
        public HttpResponseMessage SetConfig(int id, JObject config)
        {
            dynamic json = config;
            CalcRelease calcConfiguration = db.CalcRelease.Find(id);
            //Update the model with configuration and update information
            calcConfiguration.Configuration = Convert.ToString(json.data);
            calcConfiguration.User = HttpContext.Current.User.Identity.Name.ToString();
            calcConfiguration.UpdateDate = DateTime.Now;
            //update the database entity
            db.Entry(calcConfiguration).State = EntityState.Modified;
            db.SaveChanges();
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(json.data));
            return response;
        }

        /// <summary>On Calculation Button Click this method calculates the benefits and returns the json back to the front end.
        /// <para>id = CalculationID on DB Table </para>
        /// <para>moveTaskParams = JSON Config from calculation with new inputs</para>
        /// </summary>

        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(int id,  JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            string jsonString = Convert.ToString(json.data);
            //Deserialize config JSON to category object
            List<CategoryViewModel> jCategory = (List<CategoryViewModel>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<CategoryViewModel>));
            List<OutputListGroup> OutputList = new List<OutputListGroup>();
            Calculate Calculate = new Calculate();
            //Calculate Benefits
            OutputList = Calculate.OutputResults(jCategory);
            //Update Cache with new data
            repo.UpdateConfig(jCategory);
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(OutputList));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
     }
}
