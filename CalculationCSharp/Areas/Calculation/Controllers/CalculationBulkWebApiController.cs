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

        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(int id,  JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            string jsonString = Convert.ToString(json.data);
            var response = Request.CreateResponse();
            List<OutputListGroup> OutputList = new List<OutputListGroup>();
            List<List<OutputListGroup>> BulkOutputList = new List<List<OutputListGroup>>();
            bool isNameDone = false;
            foreach (var group in json.data)
            {
                List<CategoryViewModel> jCategory = (List<CategoryViewModel>)javaScriptSerializ­er.Deserialize(Convert.ToString(group), typeof(List<CategoryViewModel>));
                List<ConfigViewModel> jConfig = (List<ConfigViewModel>)javaScriptSerializ­er.Deserialize(Convert.ToString(group), typeof(List<ConfigViewModel>));
                Calculate Calculate = new Calculate();
                OutputList = Calculate.OutputResults(jCategory);
                BulkOutputList.Add(OutputList);
                
            }

            //Iterate through data list collection
            List <List <string>> propNames = new List<List<string>>();
            List<string> propLabel = new List<string>();
            List<string> propValues = new List<string>();
            int LoopCounter = 0;

            foreach (var item in BulkOutputList)
            {
                LoopCounter = 0;

                foreach (var list in item)
                {

                    LoopCounter = LoopCounter + 1;
                    if (isNameDone == false)
                    {
                        propValues.Add(list.Group);
                        propNames.Add(propValues);
                        propValues = new List<string>();
                    }
                    //Iterate through property collection
                    foreach (var prop in list.Output)
                    {
                        LoopCounter = LoopCounter + 1;
                        if (isNameDone == false)
                        {
                            propValues.Add(prop.Field);
                            propValues.Add(prop.Value);
                            propNames.Add(propValues);
                            propValues = new List<string>();
                        }
                        else
                        {
                            propValues.Add(prop.Value);
                            propNames[LoopCounter-1].Add(prop.Value);
                            propValues = new List<string>();
                        }

                    }
                }

                isNameDone = true;
            }

            response.Content = new StringContent(JsonConvert.SerializeObject(propNames));

            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

    }
}
