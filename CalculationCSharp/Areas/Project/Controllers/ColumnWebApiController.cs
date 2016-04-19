using CalculationCSharp.Areas.Project.Models;
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class ColumnWebApiController : ApiController
    {
         CalculationDBContext db = new CalculationDBContext();

        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(int? id)
        {
            var repo = new ColumnRepository();
            var response = Request.CreateResponse();

            ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(id));

            response.Content = new StringContent(JsonConvert.SerializeObject(repo.GetColumns(ProjectBoard)));

            return response;
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage CanMove(int sourceColId, int targetColId)
        {            
            var response = Request.CreateResponse();
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent(JsonConvert.SerializeObject(new { canMove = false }));

            if (sourceColId == (targetColId - 1))
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(new { canMove = true }));
            }

            return response;
        }

        [System.Web.Http.HttpPost]        
        public HttpResponseMessage MoveStory(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            var repo = new ColumnRepository();
            CalculationDBContext db = new CalculationDBContext();
            var response = Request.CreateResponse();

            if (json.updateType == "Add")
            {
                repo.AddStory((int)json.targetColId);
            }
            else if (json.updateType == "Delete")
            {
                repo.DeleteStory((int)json.storyId, (int)json.targetColId);
            }
            else if (json.updateType == "Edit")
            {
                repo.EditStory((int)json.storyId, (int)json.targetColId, json.data);
            }
            else
            {
                repo.MoveStory((int)json.storyId, (int)json.targetColId);
            }

            response.StatusCode = HttpStatusCode.OK;

            return response;
        }


    }
}
