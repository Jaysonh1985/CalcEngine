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
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);

            calcConfiguration.Configuration = Convert.ToString(json.data);
            calcConfiguration.User = HttpContext.Current.User.Identity.Name.ToString();
            calcConfiguration.UpdateDate = DateTime.Now;
            calcConfiguration.Version = calcConfiguration.Version + (decimal)0.01;

            db.Entry(calcConfiguration).State = EntityState.Modified;

            db.SaveChanges();


            CalcHistory.CalcID = calcConfiguration.ID;
            CalcHistory.Name = calcConfiguration.Name;
            CalcHistory.Scheme = calcConfiguration.Scheme;
            CalcHistory.Configuration = calcConfiguration.Configuration;
            CalcHistory.Comment = "Test";
            CalcHistory.UpdateDate = DateTime.Now;
            CalcHistory.User = calcConfiguration.User;
            CalcHistory.Version = calcConfiguration.Version;

            CalcHistories.PostCalcHistory(CalcHistory);

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
            Calculate Calculate = new Calculate();
            jCategory = Calculate.DebugResults(jCategory);
            repo.UpdateConfig(jCategory);



            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(jCategory));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
     }
}
