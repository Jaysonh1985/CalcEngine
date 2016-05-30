using CalculationCSharp.Areas.Configuration.Models;
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class ConfigWebApiController : ApiController
    {
        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        ConfigViewModel Config = new ConfigViewModel();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse();

            List<ConfigViewModel> json = repo.GetConfig(null);

            response.Content = new StringContent(JsonConvert.SerializeObject(json));

            
            return response;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;

            string jsonString = Convert.ToString(json.data);

            List<ConfigViewModel> jConfig = (List<ConfigViewModel>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<ConfigViewModel>));

            double answer = 0;

            foreach (var item in jConfig)
            {

                if(item.Function == "Input")
                {

                }
                else
                {
                    if(item.Parameter != null){

                        foreach (var param in item.Parameter)
                        {
                            string jparameters = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                            if (item.Function == "Maths")
                            {
                                Maths Maths = new Maths();
                                Maths parameters = (Maths)javaScriptSerializ­er.Deserialize(jparameters, typeof(Maths));
                                answer = Maths.Setup(jConfig, parameters, item.ID);
                            }
                        }
                        item.Output = Convert.ToString(answer);

                    }


                }
            }

            repo.UpdateConfig(jConfig);
            

            var response = Request.CreateResponse();

            response.Content = new StringContent(JsonConvert.SerializeObject(jConfig));

            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        




    }
}
