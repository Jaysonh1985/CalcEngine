using CalculationCSharp.Areas.Project.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class BoardWebApiController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var repo = new BoardRepository();
            var response = Request.CreateResponse();

            response.Content = new StringContent(JsonConvert.SerializeObject(repo.GetColumns()));
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        [HttpGet]
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

        [HttpPost]        
        public HttpResponseMessage MoveTask(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            var repo = new BoardRepository();

            if(json.updateType == "Add")
            {
                repo.AddTask((int)json.targetColId);
            }
            else if(json.updateType == "Delete")
            {
                repo.DeleteTask((int)json.taskId, (int)json.targetColId);
            }
            else if (json.updateType == "Edit")
            {
                repo.EditTask((int)json.taskId, (int)json.targetColId, json.data);
            }
            else
            {
                repo.MoveTask((int)json.taskId, (int)json.targetColId);
            }
            
                       
            var response = Request.CreateResponse();
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }


    }
}
