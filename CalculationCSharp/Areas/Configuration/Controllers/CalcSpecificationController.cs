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
using System.Text;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Net.Http.Headers;

namespace CalculationCSharp.Areas.Config.Controllers
{
    public class CalcSpecificationController : ApiController
    {
        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        [System.Web.Http.HttpPost]
        public HttpResponseMessage SpecBuild(int id,  JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            string jsonString = Convert.ToString(json.data);
            var response = Request.CreateResponse();
            List<CategoryViewModel> jCategory = (List<CategoryViewModel>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<CategoryViewModel>));

            //Iterate through data list collection
            List <List <string>> propNames = new List<List<string>>();
            List<List<string>> propParamsVal = new List<List<string>>();
            List<string> propLabel = new List<string>();
            List<string> propValues = new List<string>();
            List<string> propParams = new List<string>();
            List<string> propMathsParams = new List<string>();
            List<string> propInputParams = new List<string>();
            int LoopCounter = 0;
            LoopCounter = 0;

            foreach (var item in jCategory)
            {
                LoopCounter = 0;
                propValues.Add(item.Name);
                propValues.Add(item.Description);
                propNames.Add(propValues);
                propValues = new List<string>();

                foreach (var list in item.Functions)
                {
                    LoopCounter = LoopCounter + 1;
                    propValues.Add(Convert.ToString(list.ID));
                    propValues.Add(Convert.ToString(list.Name));
                    propValues.Add(Convert.ToString(list.Function));
                    propValues.Add(Convert.ToString(list.Type));

                    //Logic
                    List<string> logicString = new List<string>();
                    foreach (var logicvar in list.Logic)
                    {
                        logicString.Add(Convert.ToString(logicvar.Bracket1 + " " + logicvar.Input1 + " "
                            + " " + logicvar.LogicInd + " " + logicvar.Input2 + " " + logicvar.Bracket2 + " " + logicvar.Operator));
                    }

                    StringBuilder builderLogic = new StringBuilder();
                    foreach (var logiclevel in logicString)
                    {
                        // Append each int to the StringBuilder overload.
                        builderLogic.Append(logiclevel).Append(" ");
                    }
                    propValues.Add(Convert.ToString(builderLogic));

                    //Functions
                    if (list.Function != "Input")
                    {
                        if(list.Function == "Maths")
                        {
                            //Maths
                            List<string> mathString = new List<string>();
                            foreach (var Mathvar in list.Parameter)
                            {
                                Maths MathsObject = new Maths();
                                foreach (var MathvarLevel in Mathvar)
                                {
                                    if(MathvarLevel.Key == "Bracket1")
                                    {
                                        MathsObject.Bracket1 = MathvarLevel.Value; 
                                    }
                                    else if (MathvarLevel.Key == "Input1")
                                    {
                                        MathsObject.Input1 = MathvarLevel.Value;
                                    }
                                    else if (MathvarLevel.Key == "Logic")
                                    {
                                        MathsObject.Logic = MathvarLevel.Value;
                                    }
                                    else if (MathvarLevel.Key == "Input2")
                                    {
                                        MathsObject.Input2 = MathvarLevel.Value;
                                    }
                                    else if (MathvarLevel.Key == "Bracket2")
                                    {
                                        MathsObject.Bracket2 = MathvarLevel.Value;
                                    }
                                    else if (MathvarLevel.Key == "Rounding")
                                    {
                                        if(MathvarLevel.Value != null)
                                        {
                                            MathsObject.Rounding = Convert.ToInt16(MathvarLevel.Value);
                                        }
                                        else
                                        {
                                            MathsObject.Rounding = null;
                                        }
                                    }
                                    else if (MathvarLevel.Key == "RoundingType")
                                    {
                                        MathsObject.RoundingType = MathvarLevel.Value;
                                    }
                                    else if (MathvarLevel.Key == "Logic2")
                                    {
                                        MathsObject.Logic2 = MathvarLevel.Value;
                                    }
                                }
                                if(MathsObject.Rounding > 0)
                                {
                                    mathString.Add(Convert.ToString(MathsObject.Bracket1 + "" + MathsObject.Input1 + " "
                                   + MathsObject.Logic + " " + MathsObject.Input2 + "" + MathsObject.Bracket2 + " "
                                   + MathsObject.Rounding + "dp " + MathsObject.RoundingType + " " + MathsObject.Logic2));
                                }
                                else
                                {
                                    mathString.Add(Convert.ToString(MathsObject.Bracket1 + "" + MathsObject.Input1 + " "
                                   + MathsObject.Logic + " " + MathsObject.Input2 + "" + MathsObject.Bracket2 + " "
                                   + MathsObject.Rounding + "" + MathsObject.RoundingType + "" + MathsObject.Logic2));
                                }
                            }
                            StringBuilder MathStringbuilder = new StringBuilder();
                            foreach (var bitmathlevel in mathString)
                            {
                                // Append each int to the StringBuilder overload.
                                MathStringbuilder.Append(bitmathlevel).Append(" ");
                            }
                            propValues.Add(Convert.ToString(MathStringbuilder));
                            propMathsParams = new List<string>();
                        }
                        else
                        {
                            foreach (var bit in list.Parameter)
                            {
                                foreach (var bitlevel in bit)
                                {
                                    propParams.Add(Convert.ToString(bitlevel.Key));
                                    propParams.Add(":");
                                    propParams.Add(Convert.ToString(bitlevel.Value));
                                    propParams.Add("~");
                                }
                                propParams.RemoveAt(propParams.LastIndexOf("~"));
                                StringBuilder builder = new StringBuilder();
                                foreach (var bitlevel in propParams)
                                {
                                    // Append each int to the StringBuilder overload.
                                    builder.Append(bitlevel).Append(" ");
                                }
                                propValues.Add(Convert.ToString(builder));
                                propParams = new List<string>();
                            }
                        }                      
                    }
                    else
                    {
                        foreach(var InputVal in list.Parameter)
                        {
                            foreach(var InputVal1 in InputVal)
                            {
                                if(InputVal1.Key == "templateOptions")
                                {
                                    foreach(var InputVal2 in InputVal1.Value)
                                    {
                                        if(InputVal2.Key != "options")
                                        {
                                            propInputParams.Add(InputVal2.Key);
                                            propInputParams.Add(":");
                                            propInputParams.Add(Convert.ToString(InputVal2.Value));
                                            propInputParams.Add("~");
                                        }
                                    }
                                }                              
                            }
                        }
                        propInputParams.RemoveAt(propInputParams.LastIndexOf("~"));
                        StringBuilder inputbuilder = new StringBuilder();
                        foreach (var bitlevel2 in propInputParams)
                        {
                            // Append each int to the StringBuilder overload.
                            inputbuilder.Append(bitlevel2).Append(" ");
                        }
                        propValues.Add(Convert.ToString(inputbuilder));
                        propInputParams = new List<string>();
                    }
                    propNames.Add(propValues);
                    propValues = new List<string>();
                }
            }
            response.Content = new StringContent(JsonConvert.SerializeObject(propNames));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
