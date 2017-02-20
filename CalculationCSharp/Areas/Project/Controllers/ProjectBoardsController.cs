// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CalculationCSharp.Models;
using System.Web;
using System.Web.Script.Serialization;
using CalculationCSharp.Areas.Project.Models;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class ProjectBoardsController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/ProjectBoards
        public IQueryable<ProjectBoards> GetProjectBoard()
        {
            return db.ProjectBoards;
        }

        // GET: api/ProjectBoards/5
        [ResponseType(typeof(ProjectBoards))]
        public IHttpActionResult GetProjectBoard(int id)
        {
            ProjectBoards projectBoard = db.ProjectBoards.Find(id);
            if (projectBoard == null)
            {
                return NotFound();
            }

            return Ok(projectBoard);
        }

        // PUT: api/ProjectBoards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProjectBoard(int id, ProjectBoards projectBoard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectBoard.BoardId)
            {
                return BadRequest();
            }
            projectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
            projectBoard.UpdateDate = DateTime.Now;

            db.Entry(projectBoard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectBoardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProjectBoards
        [ResponseType(typeof(ProjectBoards))]
        public IHttpActionResult PostProjectBoard(ProjectBoards projectBoard)
        {           
            if (projectBoard.BoardId > 0)
            {
                BoardRepository repo = new BoardRepository();
                projectBoard = repo.CopyBoard(projectBoard, projectBoard.BoardId);
            }
            else
            {
                projectBoard.UpdateDate = DateTime.Now;
                projectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
                db.Configuration.ProxyCreationEnabled = false;
            }
            db.ProjectBoards.Add(projectBoard);
            db.SaveChanges();

            projectBoard.ProjectColumns = null;
            return CreatedAtRoute("DefaultApi", new { id = projectBoard.BoardId }, projectBoard);
        }
        // DELETE: api/ProjectBoards/5
        [ResponseType(typeof(ProjectBoards))]
        public IHttpActionResult DeleteProjectBoard(int id)
        {
            var originalBoard = db.ProjectBoards
            .Where(p => p.BoardId == id)
            .Include(p => p.ProjectColumns)
            .SingleOrDefault();

            ProjectBoards projectBoard = db.ProjectBoards.Find(id);

            foreach (var Column in originalBoard.ProjectColumns.ToList())
            {
                foreach (var Story in Column.ProjectStories.ToList())
                {
                    foreach (var Comment in Story.ProjectComments.ToList())
                    {
                        db.ProjectComments.Remove(Comment);
                    }
                    foreach (var Task in Story.ProjectTasks.ToList())
                    {
                        db.ProjectTasks.Remove(Task);
                    }
                    foreach (var Update in Story.ProjectUpdates.ToList())
                    {
                        db.ProjectUpdates.Remove(Update);
                    }
                    db.ProjectStories.Remove(Story);
                }
                db.ProjectColumns.Remove(Column);
            }
            db.ProjectBoards.Remove(originalBoard);

            if (projectBoard == null)
            {
                return NotFound();
            }

            db.ProjectBoards.Remove(projectBoard);
            db.SaveChanges();

            return Ok(projectBoard);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectBoardExists(int id)
        {
            return db.ProjectBoards.Count(e => e.BoardId == id) > 0;
        }
    }
}