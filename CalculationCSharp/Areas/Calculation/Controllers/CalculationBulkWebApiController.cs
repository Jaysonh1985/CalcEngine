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
using System.Text;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Net.Http.Headers;

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class CalculationBulkWebApiController : ApiController
    {
        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //// GET api/<controller>
        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage Get(int? id)
        //{
        //    var response = Request.CreateResponse();
        //    CalcRelease calcConfiguration = db.CalcRelease.Find(id);
        //    if (calcConfiguration == null)
        //    {
                
        //        List<CategoryViewModel> json = repo.GetConfig(null);
        //        response.Content = new StringContent(JsonConvert.SerializeObject(json));
        //    }
        //    else
        //    {
        //        if (calcConfiguration.Configuration == null)
        //        {
        //            List<CategoryViewModel> json = repo.GetConfig(null);
        //            response.Content = new StringContent(JsonConvert.SerializeObject(json));
        //        }
        //        else
        //        {
                   
        //            response.Content = new StringContent(calcConfiguration.Configuration);
        //        }
                
        //    }
            
        //    return response;
        //}


        //[System.Web.Http.HttpPut]
        //public HttpResponseMessage SetConfig(int id, JObject config)
        //{

        //    dynamic json = config;

        //    var response = Request.CreateResponse();
        //    CalcRelease calcConfiguration = db.CalcRelease.Find(id);

        //    calcConfiguration.Configuration = Convert.ToString(json.data);
        //    calcConfiguration.User = HttpContext.Current.User.Identity.Name.ToString();
        //    calcConfiguration.UpdateDate = DateTime.Now;

        //    db.Entry(calcConfiguration).State = EntityState.Modified;

        //    db.SaveChanges();
        //    response.Content = new StringContent(JsonConvert.SerializeObject(json.data));

        //    return response;
        //}


        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(int id,  JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            string jsonString = Convert.ToString(json.data);
            var response = Request.CreateResponse();
            List<OutputListGroup> OutputList = new List<OutputListGroup>();
            List<List<OutputListGroup>> BulkOutputList = new List<List<OutputListGroup>>();
            foreach (var group in json.data)
            {
                List<CategoryViewModel> jCategory = (List<CategoryViewModel>)javaScriptSerializ­er.Deserialize(Convert.ToString(group), typeof(List<CategoryViewModel>));
                List<ConfigViewModel> jConfig = (List<ConfigViewModel>)javaScriptSerializ­er.Deserialize(Convert.ToString(group), typeof(List<ConfigViewModel>));

                
                
                Calculate Calculate = new Calculate();

                OutputList = Calculate.OutputResults(jCategory);
                BulkOutputList.Add(OutputList);

                
            }

            StringBuilder sb = new StringBuilder();
            List<string> propNames;
            List<string> propValues;
            bool isNameDone = false;

            //Iterate through data list collection
            propNames = new List<string>();
            propValues = new List<string>();

            foreach (var item in BulkOutputList)
            {
                foreach (var list in item)
                {
                    sb.AppendLine("");

                    //Iterate through property collection
                    foreach (var prop in list.Output)
                    {
                        if (!isNameDone) propNames.Add(prop.Field);

                    }

                    //Add line for Names
                    string line = string.Empty;
                    if (!isNameDone)
                    {
                        line = string.Join(",", propNames);
                        sb.AppendLine(line);
                        sb.AppendLine("");
                        isNameDone = true;
                    }
                    foreach (var col in OutputList)
                    {
                        foreach (var story in col.Output)
                        {
                            propValues.Add(story.Value);
                            line = string.Join(",", propValues);
                            sb.Append(line);
                            sb.AppendLine("");
                        }

                    }

                }
            }

            var csv = Convert.ToString(sb);

            response.Content = new StringContent(JsonConvert.SerializeObject(csv));

            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

    }
}
