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
    public class BoardWebApiController : ApiController
    {
        BoardRepository repo = new BoardRepository();

        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            ProjectBoard ProjectBoard = repo.GetBoard(json);
            var response = Request.CreateResponse();

            //response.Content = new StringContent(JsonConvert.SerializeObject(repo.GetColumns()));
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        [System.Web.Http.HttpPost]        
        public HttpResponseMessage UpdateBoard(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;


            var response = Request.CreateResponse();

            if (json.boardId == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            ProjectBoard ProjectBoard = repo.GetBoard(json);

            if (ProjectBoard == null)
            {
                repo.AddBoard(json);
            }
            else
            {
                repo.UpdateBoard(json);
            }

            response.StatusCode = HttpStatusCode.OK;

            return response;
        }


    }
}
