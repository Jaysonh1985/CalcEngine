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

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class ProjectBoardsController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/ProjectBoards
        public IQueryable<ProjectBoard> GetProjectBoard()
        {
            return db.ProjectBoard;
        }

        // GET: api/ProjectBoards/5
        [ResponseType(typeof(ProjectBoard))]
        public IHttpActionResult GetProjectBoard(int id)
        {
            ProjectBoard projectBoard = db.ProjectBoard.Find(id);
            if (projectBoard == null)
            {
                return NotFound();
            }

            return Ok(projectBoard);
        }

        // PUT: api/ProjectBoards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProjectBoard(int id, ProjectBoard projectBoard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectBoard.ID)
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
        [ResponseType(typeof(ProjectBoard))]
        public IHttpActionResult PostProjectBoard(ProjectBoard projectBoard)
        {
            projectBoard.UpdateDate = DateTime.Now;
            projectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}           
            db.ProjectBoard.Add(projectBoard);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = projectBoard.ID }, projectBoard);
        }

        // DELETE: api/ProjectBoards/5
        [ResponseType(typeof(ProjectBoard))]
        public IHttpActionResult DeleteProjectBoard(int id)
        {
            ProjectBoard projectBoard = db.ProjectBoard.Find(id);
            if (projectBoard == null)
            {
                return NotFound();
            }

            db.ProjectBoard.Remove(projectBoard);
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
            return db.ProjectBoard.Count(e => e.ID == id) > 0;
        }
    }
}