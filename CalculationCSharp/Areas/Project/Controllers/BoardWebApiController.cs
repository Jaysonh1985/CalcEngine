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
        public HttpResponseMessage MoveStory(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            var repo = new BoardRepository();

            if (json.task == "true")
            {

                repo.AddTask((int)json.storyId, (int)json.targetColId, json.data);


            }
            else
            {
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
            }


            var response = Request.CreateResponse();
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }


    }
}
