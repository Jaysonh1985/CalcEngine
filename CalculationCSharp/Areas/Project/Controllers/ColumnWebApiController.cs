// Copyright (c) 2016 Project AIM
using CalculationCSharp.Areas.Project.Models;
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class ColumnWebApiController : ApiController
    {
        CalculationDBContext db = new CalculationDBContext();
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(int? id)
        {
            var response = Request.CreateResponse();
            ProjectBoards ProjectBoard = db.ProjectBoards.Find(Convert.ToInt32(id));
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = ProjectBoard.Configuration;
            if(ProjectBoard.ProjectColumns.Count == 0)
            {
                var columns = new List<ProjectColumns>();
                //var stories = new List<ProjectStories>();
                columns.Add(new ProjectColumns { Description = "Backlog Column", Name = "Backlog", ProjectBoard = ProjectBoard, UpdateDate = DateTime.Now, });
                columns.Add(new ProjectColumns { Description = "In Progress Column",  Name = "In Progress", ProjectBoard = ProjectBoard, UpdateDate = DateTime.Now});
                columns.Add(new ProjectColumns { Description = "Pending Column",  Name = "Pending", ProjectBoard = ProjectBoard, UpdateDate = DateTime.Now });
                columns.Add(new ProjectColumns { Description = "Release Column",  Name = "Release", ProjectBoard = ProjectBoard, UpdateDate = DateTime.Now });
                ProjectBoard.ProjectColumns = columns;
                db.SaveChanges();

            }
            var ColumnsReturns = ProjectBoard.ProjectColumns
            .Select(o => new ProjectColumns
            {
                Name = o.Name,
                ColumnId = o.ColumnId,
                UpdateDate = o.UpdateDate,
                Description = o.Description,
                ProjectStories = o.ProjectStories

            }).ToList();

            foreach(var col in ColumnsReturns)
            {
                foreach(var Story in col.ProjectStories)
                {
                    Story.ProjectColumns = null;

                    foreach (var Comment in Story.ProjectComments)
                    {
                        Comment.ProjectStories = null;
                    }
                    foreach (var Task in Story.ProjectTasks)
                    {
                        Task.ProjectStories = null;
                    }
                    foreach (var Update in Story.ProjectUpdates)
                    {
                        Update.ProjectStories = null;
                    }
                }

            }

            response.Content = new StringContent(JsonConvert.SerializeObject(ColumnsReturns, Formatting.Indented));
            return response;
        }
    }
}
