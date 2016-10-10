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


        [System.Web.Http.HttpPut]
        public HttpResponseMessage SetConfig(int id, JObject config)
        {

            dynamic json = config;

            var response = Request.CreateResponse();
            CalcRelease calcConfiguration = db.CalcRelease.Find(id);

            calcConfiguration.Configuration = Convert.ToString(json.data);
            calcConfiguration.User = HttpContext.Current.User.Identity.Name.ToString();
            calcConfiguration.UpdateDate = DateTime.Now;

            db.Entry(calcConfiguration).State = EntityState.Modified;

            db.SaveChanges();
            response.Content = new StringContent(JsonConvert.SerializeObject(json.data));

            return response;
        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(int id,  JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            string jsonString = Convert.ToString(json.data);
            List<CategoryViewModel> jCategory = (List<CategoryViewModel>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<CategoryViewModel>));
            List<ConfigViewModel> jConfig = (List<ConfigViewModel>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<ConfigViewModel>));
            List<OutputListGroup> OutputList = new List<OutputListGroup>();
            Calculate Calculate = new Calculate();

            OutputList = Calculate.OutputResults(jCategory);
            repo.UpdateConfig(jCategory);
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(OutputList));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
     }
}
