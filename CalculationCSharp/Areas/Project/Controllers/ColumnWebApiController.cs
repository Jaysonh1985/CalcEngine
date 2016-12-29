// Copyright (c) 2016 Project AIM
using CalculationCSharp.Areas.Project.Models;
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    }
}
