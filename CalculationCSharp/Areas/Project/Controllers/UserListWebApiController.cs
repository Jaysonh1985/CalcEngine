using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        public async System.Threading.Tasks.Task<HttpResponseMessage> Get()
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var role = await RoleManager.FindByNameAsync("Project");
            var listUsers = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            //Create object response
            var response = Request.CreateResponse();
            response.Content = new StringContent(JsonConvert.SerializeObject(listUsers));
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
