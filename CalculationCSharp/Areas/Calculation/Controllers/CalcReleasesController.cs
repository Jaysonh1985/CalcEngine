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

namespace CalculationCSharp.Areas.Calculation.Controllers
{
    public class CalcReleasesController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcReleases
        public IQueryable<CalcRelease> GetCalcRelease()
        {
            return db.CalcRelease;
        }

        // GET: api/CalcReleases/5
        [ResponseType(typeof(CalcRelease))]
        public IHttpActionResult GetCalcRelease(int id)
        {
            CalcRelease calcRelease = db.CalcRelease.Find(id);
            if (calcRelease == null)
            {
                return NotFound();
            }

            return Ok(calcRelease);
        }

        // PUT: api/CalcReleases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalcRelease(int id, CalcRelease calcRelease)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calcRelease.ID)
            {
                return BadRequest();
            }

            db.Entry(calcRelease).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalcReleaseExists(id))
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

        // POST: api/CalcReleases
        [ResponseType(typeof(CalcRelease))]
        public IHttpActionResult PostCalcRelease(CalcRelease calcRelease)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CalcRelease.Add(calcRelease);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = calcRelease.ID }, calcRelease);
        }

        // DELETE: api/CalcReleases/5
        [ResponseType(typeof(CalcRelease))]
        public IHttpActionResult DeleteCalcRelease(int id)
        {
            CalcRelease calcRelease = db.CalcRelease.Find(id);
            if (calcRelease == null)
            {
                return NotFound();
            }

            db.CalcRelease.Remove(calcRelease);
            db.SaveChanges();

            return Ok(calcRelease);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalcReleaseExists(int id)
        {
            return db.CalcRelease.Count(e => e.ID == id) > 0;
        }
    }
}