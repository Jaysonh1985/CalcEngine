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
using CalculationCSharp.Models.Calculation;

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
            BulkOutputListBuilder BulkBuilder = new BulkOutputListBuilder();

            //Create object response
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject((BulkBuilder.BulkOutput(BulkOutputList))));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

    }
}
