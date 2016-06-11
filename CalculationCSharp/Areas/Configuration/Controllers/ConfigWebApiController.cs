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

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class ConfigWebApiController : ApiController
    {
        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

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
                            string logic = null;
                            bool logicparse = true;

                            foreach (var bit in item.Logic)
                            {

                                string inputA = bit.Input1;
                                string Logic = bit.LogicInd;
                                string inputB = bit.Input2;

                                logic = "if(" + inputA + Logic + inputB + ",true,false)";
                                Expression ex = new Expression(logic);
                                logicparse = Convert.ToBoolean(ex.Evaluate());
                            }
                            


                            if (logicparse == true)
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
                                        dynamic InputA = Config.VariableReplace(jCategory, parameters.Input1, item.ID);
                                        dynamic InputB = Config.VariableReplace(jCategory, parameters.Input2, item.ID);
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

                                item.Type = "Decimal";
                                item.Output = Convert.ToString(answer);

                                if (item.ExpectedResult == null || item.ExpectedResult == "")
                                {
                                    item.Pass = "true";

                                }
                                else if (item.ExpectedResult == item.Output)
                                {
                                    item.Pass = "true";
                                }
                                else
                                {
                                    item.Pass = "false";
                                }
                            }
                            else
                            {
                                item.Pass = "miss";
                            }
                        }
                    }
                }
            }
            repo.UpdateConfig(jCategory);
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(jCategory));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
     }
}
