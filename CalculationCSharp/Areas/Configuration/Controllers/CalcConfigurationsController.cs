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

namespace CalculationCSharp.Areas.Configuration.Controllers
{
    public class CalcConfigurationsController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcConfigurations
        public IQueryable<CalcConfiguration> GetCalcConfiguration()
        {
            return db.CalcConfiguration;
        }

        // GET: api/CalcConfigurations/5
        [ResponseType(typeof(CalcConfiguration))]
        public IHttpActionResult GetCalcConfiguration(int id)
        {
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);
            if (calcConfiguration == null)
            {
                return NotFound();
            }

            return Ok(calcConfiguration);
        }

        // PUT: api/CalcConfigurations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalcConfiguration(int id, CalcConfiguration calcConfiguration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calcConfiguration.ID)
            {
                return BadRequest();
            }

            db.Entry(calcConfiguration).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalcConfigurationExists(id))
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

        // POST: api/CalcConfigurations
        [ResponseType(typeof(CalcConfiguration))]
        public IHttpActionResult PostCalcConfiguration(CalcConfiguration calcConfiguration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            calcConfiguration.UpdateDate = DateTime.Now;
            db.CalcConfiguration.Add(calcConfiguration);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = calcConfiguration.ID }, calcConfiguration);
        }

        // DELETE: api/CalcConfigurations/5
        [ResponseType(typeof(CalcConfiguration))]
        public IHttpActionResult DeleteCalcConfiguration(int id)
        {
            CalcConfiguration calcConfiguration = db.CalcConfiguration.Find(id);
            if (calcConfiguration == null)
            {
                return NotFound();
            }

            db.CalcConfiguration.Remove(calcConfiguration);
            db.SaveChanges();

            return Ok(calcConfiguration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalcConfigurationExists(int id)
        {
            return db.CalcConfiguration.Count(e => e.ID == id) > 0;
        }
    }
}