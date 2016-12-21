using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class UserListWebApiController : ApiController
    {
        CalculationCSharp.Models.ApplicationDbContext context = new CalculationCSharp.Models.ApplicationDbContext();
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            var listUsers = context.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            //Create object response
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(listUsers));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
