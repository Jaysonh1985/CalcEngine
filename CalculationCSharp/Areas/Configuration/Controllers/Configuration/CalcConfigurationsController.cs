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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CalculationCSharp.Areas.Configuration.Controllers
{
    public class CalcConfigurationsController : ApiController
    {
        /// <summary>Controller for updating the calcConfigurations table on the db.
        /// </summary>

        CalcHistoriesController CalcHistories = new CalcHistoriesController();
        CalcHistory CalcHistory = new CalcHistory();
        CalculationCSharp.Models.ApplicationDbContext context = new CalculationCSharp.Models.ApplicationDbContext();
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcConfigurations
        public IQueryable<CalcConfiguration> GetCalcConfiguration()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            string[] myInClause = null;
            if (user.Scheme != null)
            {
                myInClause = user.Scheme.Split(',');
                return db.CalcConfiguration.Where(s => myInClause.Contains(s.Scheme)).OrderBy(s => s.Scheme);
            }
            else
            {
                return null;
            }
            


        }
        /// <summary>Get list of Calcs available in the Calculation System.
        /// <para>id = ID on DB Table </para>
        /// </summary>
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
        /// <summary>Updates the configuration on the database.
        /// <para>id = ID on DB Table </para>
        /// <para>calcConfiguration = Database Object entity </para>
        /// </summary>
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

            calcConfiguration.UpdateDate = DateTime.Now;
            calcConfiguration.User = HttpContext.Current.User.Identity.Name.ToString();

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
        /// <summary>Posts new configuration on the database.
        /// <para>calcConfiguration = Database Object entity </para>
        /// </summary>
        // POST: api/CalcConfigurations
        [ResponseType(typeof(CalcConfiguration))]
        public IHttpActionResult PostCalcConfiguration(CalcConfiguration calcConfiguration)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            calcConfiguration.UpdateDate = DateTime.Now;
            calcConfiguration.User = HttpContext.Current.User.Identity.Name.ToString();
            db.CalcConfiguration.Add(calcConfiguration);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = calcConfiguration.ID }, calcConfiguration);
        }
        /// <summary>Posts new configuration on the database.
        /// <para>id = ID on DB Table </para>
        /// </summary>
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
        /// <summary>Disposes the controller if error has occured.
        /// </summary>
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