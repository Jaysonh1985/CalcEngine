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
using CalculationCSharp.Areas.Dashboard.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace CalculationCSharp.Areas.Dashboard.Controllers
{
    public class CalcTeamMembersController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcTeamMembers
        public IQueryable<CalcTeamMembers> GetCalcTeamMembers()
        {
            return db.CalcTeamMembers;
        }

        // GET: api/CalcTeamMembers/5
        [ResponseType(typeof(CalcTeamMembers))]
        public IHttpActionResult GetCalcTeamMembers(int id)
        {
            CalcTeamMembers calcTeamMembers = db.CalcTeamMembers.Find(id);
            if (calcTeamMembers == null)
            {
                return NotFound();
            }

            return Ok(calcTeamMembers);
        }

        // PUT: api/CalcTeamMembers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalcTeamMembers(int id, CalcTeamMembers calcTeamMembers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calcTeamMembers.CalcTeamMemberID)
            {
                return BadRequest();
            }

            db.Entry(calcTeamMembers).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalcTeamMembersExists(id))
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

        // POST: api/CalcTeamMembers
        [ResponseType(typeof(CalcTeamMembers))]
        public IHttpActionResult PostCalcTeamMembers(CalcTeamMembersViewModel calcTeamMembers)
        {
            var response = Request.CreateResponse();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByEmailAsync(calcTeamMembers.UserId).Result;
            if(user == null)
            {
                return NotFound();
            }
            CalcTeams calcTeam = db.CalcTeams.Find(calcTeamMembers.CalcTeams);
            if (calcTeam == null)
            {
                return NotFound();
            }
            CalcTeamMembers findDuplicates = db.CalcTeamMembers.Where(b => b.UserId == user.Id && b.CalcTeams.CalcTeamID == calcTeam.CalcTeamID).FirstOrDefault();
            if(findDuplicates != null)
            {
                return NotFound();
            }
            CalcTeamMembers CalcTeamMember = new CalcTeamMembers();
            CalcTeamMember.CalcTeams = calcTeam;
            CalcTeamMember.UserId = user.Id;

            db.CalcTeamMembers.Add(CalcTeamMember);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = CalcTeamMember.CalcTeamMemberID },"");
        }

        // DELETE: api/CalcTeamMembers/5
        [ResponseType(typeof(CalcTeamMembers))]
        public IHttpActionResult DeleteCalcTeamMembers(int id)
        {
            CalcTeamMembers calcTeamMembers = db.CalcTeamMembers.Find(id);
            if (calcTeamMembers == null)
            {
                return NotFound();
            }

            db.CalcTeamMembers.Remove(calcTeamMembers);
            db.SaveChanges();

            return Ok(calcTeamMembers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalcTeamMembersExists(int id)
        {
            return db.CalcTeamMembers.Count(e => e.CalcTeamMemberID == id) > 0;
        }
    }
}