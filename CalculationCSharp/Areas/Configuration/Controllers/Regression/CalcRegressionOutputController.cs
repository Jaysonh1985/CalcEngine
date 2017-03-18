using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CalculationCSharp.Models.Calculation;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Controllers
{
    public class CalcRegressionOutputController : ApiController
    {
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfig(int id, JObject moveTaskParams)
        {
            //Associate the JSON string to an object
            dynamic json = moveTaskParams;
            List<List<OutputListGroup>> BulkOutputList = new List<List<OutputListGroup>>();
            foreach (var group in json.data)
            {
                List<OutputListGroup> OutputGroup = new JavaScriptSerializer().Deserialize<List<OutputListGroup>>(Convert.ToString(group));
                BulkOutputList.Add(OutputGroup);
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
