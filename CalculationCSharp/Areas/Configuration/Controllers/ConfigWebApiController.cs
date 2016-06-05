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
using NCalc;
using Antlr.Runtime.Debug;
using System.Text;

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class ConfigWebApiController : ApiController
    {
        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse();

            List<CategoryViewModel> json = repo.GetConfig(null);

            response.Content = new StringContent(JsonConvert.SerializeObject(json));

            
            return response;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;

            string jsonString = Convert.ToString(json.data);

            List<CategoryViewModel> jCategory = (List<CategoryViewModel>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<CategoryViewModel>));
            List<ConfigViewModel> jConfig = (List<ConfigViewModel>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<ConfigViewModel>));

            decimal answer = 0;

            foreach(var group in jCategory)
            {
                foreach (var item in group.Functions)
                {

                    if (item.Function == "Input")
                    {

                    }
                    else
                    {
                        if (item.Parameter != null)
                        {

                            foreach (var param in item.Parameter)
                            {
                                string jparameters = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                                if (item.Function == "Maths")
                                {

                                    string formula = null;

                                    Maths Maths = new Maths();
                                    Maths parameters = (Maths)javaScriptSerializ­er.Deserialize(jparameters, typeof(Maths));

                                    CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

                                    dynamic InputA = Config.VariableReplace(jConfig, parameters.Input1, item.ID);
                                    dynamic InputB = Config.VariableReplace(jConfig, parameters.Input2, item.ID);

                                    string Input1 = Convert.ToString(InputA);
                                    string Logic = Convert.ToString(parameters.Logic);
                                    string Input2 = Convert.ToString(InputB);
                                    string Rounding = Convert.ToString(parameters.Rounding);

                                    if (Rounding == "0")
                                    {
                                        Rounding = "2";
                                    }

                                    if (Logic == "Pow")
                                    {
                                        formula = Logic + '(' + Input1 + ',' + Input2 + ')';
                                    }
                                    else
                                    {
                                        formula = Input1 + Logic + Input2;
                                    }

                                    //Apply rounding
                                    formula = "Round(" + formula + "," + Rounding + ")";
                                    Expression e = new Expression(formula);
                                    var Calculation = e.Evaluate();
                                    answer = Convert.ToDecimal(Calculation);
                                }
                            }
                            item.Output = Convert.ToString(answer);

                        }


                    }
                }


            }



            repo.UpdateConfig(jCategory);


            var response = Request.CreateResponse();

            response.Content = new StringContent(JsonConvert.SerializeObject(jConfig));

            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        




    }
}
