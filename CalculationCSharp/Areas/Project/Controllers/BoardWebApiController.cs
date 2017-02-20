// Copyright (c) 2016 Project AIM
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
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using System.Collections.Generic;
using CalculationCSharp.Areas.Project.Models.ViewModels;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class BoardWebApiController : ApiController
    {
        BoardRepository repo = new BoardRepository();
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            var repo = new BoardRepository();
            var response = Request.CreateResponse();
            var ProjectBoardRepo = repo.GetBoards();

            List<ProjectBoardViewModel> ProjectBoards = new List<ProjectBoardViewModel>();

            foreach (var Board in ProjectBoardRepo)
            {
                ProjectBoards.Add(new ProjectBoardViewModel { BoardId = Board.BoardId, Client = Board.Client, Name = Board.Name, UpdateDate = Board.UpdateDate, User = Board.User });
            }

            response.Content = new StringContent(JsonConvert.SerializeObject(ProjectBoards));
            HttpContext.Current.Cache.Remove("columns");           
            return response;
        }
        [System.Web.Http.HttpPut]
        public HttpResponseMessage Update(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            string data = Convert.ToString(json.data);
            var response = Request.CreateResponse();
            if (json.boardId == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            ProjectBoards ProjectBoard = repo.GetBoard(json);
            ProjectBoard = repo.UpdateBoard(json);

            List<ProjectColumnViewModel> ProjectColumns = new List<ProjectColumnViewModel>();

            foreach(var Column in ProjectBoard.ProjectColumns)
            {
                ProjectColumns.Add(new ProjectColumnViewModel { ColumnId = Column.ColumnId, Description = Column.Description, Name = Column.Name, UpdateDate = DateTime.Now, ProjectStories = Column.ProjectStories }); 
            }
            foreach (var Column in ProjectColumns)
            {
               foreach(var Story in Column.ProjectStories)
                {
                    Story.ProjectColumns = null;
                    if(Story.ProjectComments != null)
                    {
                        foreach (var Comment in Story.ProjectComments)
                        {
                            Comment.ProjectStories = null;
                        }
                    }

                    if(Story.ProjectTasks != null)
                    {
                        foreach (var Task in Story.ProjectTasks)
                        {
                            Task.ProjectStories = null;
                        }
                    }
                    if(Story.ProjectUpdates != null)
                    {
                        foreach (var Update in Story.ProjectUpdates)
                        {
                            Update.ProjectStories = null;
                        }
                    }
                }

            }
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent(JsonConvert.SerializeObject(ProjectColumns));
            return response;
        }

        [System.Web.Http.HttpPost]        
        public HttpResponseMessage Create(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            string data = Convert.ToString(json.data);
            var response = Request.CreateResponse();
            if (json.boardId == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            ProjectBoards ProjectBoard = repo.GetBoard(json);
            if (ProjectBoard == null)
            {
                repo.AddBoard(json);
            }

            response.StatusCode = HttpStatusCode.OK;
            return response;
        }       
    }
}
