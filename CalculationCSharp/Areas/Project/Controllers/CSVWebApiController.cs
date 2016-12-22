using CalculationCSharp.Areas.Project.Models;
using CalculationCSharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class CSVWebApiController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage CSV(int? id)
        {
            ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(id));
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = Convert.ToString(ProjectBoard.Configuration);
            List<Column> columns = (List<Column>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<Column>));
            StringBuilder sb = new StringBuilder();
            //Iterate through data list collection
            List<List<string>> propNames = new List<List<string>>();
            List<string> propValues = new List<string>();
            //Loop to output the values in the csv
            int LoopCounter = 0;
            propValues.Add("ColumnID");
            propValues.Add("ColumnName");
            propValues.Add("ActivityID");
            propValues.Add("ActivityName");
            propValues.Add("Description");
            propValues.Add("AcceptanceCriteria");
            propValues.Add("Complexity");
            propValues.Add("DueDate");
            propValues.Add("Effort");
            propValues.Add("ElapsedTime");
            propValues.Add("Moscow");
            propValues.Add("RAG");
            propValues.Add("RequestedBy");
            propValues.Add("RequestedDate");
            propValues.Add("StartDate");
            propValues.Add("SLA");
            propValues.Add("Timebox");
            propValues.Add("CurrentUser");
            propNames.Add(propValues);
            propValues = new List<string>();
            //Create CSV of the output results
            foreach (var item in columns)
            {
                //Iterate through property collection
                foreach (var prop in item.Stories)
                {
                    //Sets the row label
                    LoopCounter = LoopCounter + 1;
                    propValues.Add(Convert.ToString(item.Id));
                    propValues.Add(Convert.ToString(item.Name));
                    propValues.Add(Convert.ToString(prop.Id));
                    propValues.Add(Convert.ToString(prop.Name));
                    propValues.Add(Convert.ToString(prop.Description));
                    propValues.Add(Convert.ToString(prop.AcceptanceCriteria));
                    propValues.Add(Convert.ToString(prop.Complexity));
                    propValues.Add(Convert.ToString(prop.DueDate));
                    propValues.Add(Convert.ToString(prop.Effort));
                    propValues.Add(Convert.ToString(prop.ElapsedTime));
                    propValues.Add(Convert.ToString(prop.Moscow));
                    propValues.Add(Convert.ToString(prop.RAG));
                    propValues.Add(Convert.ToString(prop.Requested));
                    propValues.Add(Convert.ToString(prop.RequestedDate));
                    propValues.Add(Convert.ToString(prop.StartDate));
                    propValues.Add(Convert.ToString(prop.SLADays));
                    propValues.Add(Convert.ToString(prop.Timebox));
                    propValues.Add(Convert.ToString(prop.User));
                    propNames.Add(propValues);
                    propValues = new List<string>();
                }

            }

            //Create object response
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(propNames));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

    }
}
