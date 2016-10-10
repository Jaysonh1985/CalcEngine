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
    public class CalculationBulkWebApiController : ApiController
    {
        /// <summary>Controller for running bulk calculations from Calculation main screen.
        /// </summary>
        ConfigRepository repo = new ConfigRepository();
        CalculationDBContext db = new CalculationDBContext();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        /// <summary>Get list of Calcs available in the Calculation System.
        /// <para>id = CalculationID on DB Table </para>
        /// <para>moveTaskParams = JSON config assiociated with the configuration</para>
        /// </summary>

        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(int id,  JObject moveTaskParams)
        {
            //Associate the JSON string to an object
            dynamic json = moveTaskParams;
            //This logic displays for discovering which data is the label for the row not the value
            bool isNameDone = false;
            //Create Lists for the calculation
            List<OutputListGroup> OutputList = new List<OutputListGroup>();
            List<List<OutputListGroup>> BulkOutputList = new List<List<OutputListGroup>>();
            //Builds calculation and creates a further object with all of the calculations
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
            List<string> propValues = new List<string>();
            //Loop to output the values in the csv
            int LoopCounter = 0;
            //Create CSV of the output results
            foreach (var item in BulkOutputList)
            {
                LoopCounter = 0;
                foreach (var list in item)
                {
                    LoopCounter = LoopCounter + 1;
                    //Sets the Group in the output
                    if (isNameDone == false)
                    {
                        propValues.Add(list.Group);
                        propNames.Add(propValues);
                        propValues = new List<string>();
                    }
                    //Iterate through property collection
                    foreach (var prop in list.Output)
                    {
                        //Sets the row label
                        LoopCounter = LoopCounter + 1;
                        if (isNameDone == false)
                        {
                            propValues.Add(prop.Field);
                            propValues.Add(prop.Value);
                            propNames.Add(propValues);
                            propValues = new List<string>();
                        }
                        //sets the row value
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
            //Create object response
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(propNames));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

    }
}
