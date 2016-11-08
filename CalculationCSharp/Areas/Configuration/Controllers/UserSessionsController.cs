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

namespace CalculationCSharp.Areas.Configuration.Controllers
{
    public class UserSessionsController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/UserSessions
        public IQueryable<UserSession> GetUserSession()
        {
            return db.UserSession;
        }

        // GET: api/UserSessions/5
        [ResponseType(typeof(UserSession))]
        public IHttpActionResult GetUserSession(int id)
        {
            string user = HttpContext.Current.User.Identity.Name.ToString();

            var List = db.UserSession.Where(i => i.Record == id && i.Username != user);
            if (List.Count() == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Ok(List.First());
            }
  
        }

        // PUT: api/UserSessions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserSession(int id, UserSession userSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userSession.ID)
            {
                return BadRequest();
            }

            db.Entry(userSession).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserSessionExists(id))
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

        // POST: api/UserSessions
        [ResponseType(typeof(UserSession))]
        public IHttpActionResult PostUserSession(UserSession userSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserSession.Add(userSession);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userSession.ID }, userSession);
        }

        // DELETE: api/UserSessions/5
        [ResponseType(typeof(UserSession))]
        public IHttpActionResult DeleteUserSession(int id)
        {
            var List = db.UserSession.Where(i => i.Record == id);
            UserSession UsersessionList = List.First();
            db.UserSession.Remove(UsersessionList);
            db.SaveChanges();

            return Ok(UsersessionList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserSessionExists(int id)
        {
            return db.UserSession.Count(e => e.ID == id) > 0;
        }
    }
}