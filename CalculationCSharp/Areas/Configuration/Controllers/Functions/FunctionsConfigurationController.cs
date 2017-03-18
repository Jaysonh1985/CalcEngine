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
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CalculationCSharp.Areas.Configuration.Controllers
{
    public class FunctionsConfigurationController : ApiController
    {
        /// <summary>Controller for updating the calcConfigurations table on the db.
        /// </summary>
        CalculationCSharp.Models.ApplicationDbContext context = new CalculationCSharp.Models.ApplicationDbContext();
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcConfigurations
        public IQueryable<FunctionConfiguration> GetCalcFunction()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            string[] myInClause = null;
            if (user.Scheme != null)
            {
                myInClause = user.Scheme.Split(',');
                return db.FunctionConfiguration.Where(s => myInClause.Contains(s.Scheme)).OrderBy(s => s.Scheme);
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
        [ResponseType(typeof(FunctionConfiguration))]
        public IHttpActionResult GetCalcFunction(int id)
        {
            FunctionConfiguration calcFunction = db.FunctionConfiguration.Find(id);
            if (calcFunction == null)
            {
                return NotFound();
            }

            return Ok(calcFunction);
        }
        /// <summary>Updates the configuration on the database.
        /// <para>id = ID on DB Table </para>
        /// <para>calcConfiguration = Database Object entity </para>
        /// </summary>
        // PUT: api/CalcConfigurations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalcFunction(int id, FunctionConfiguration calcFunction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calcFunction.ID)
            {
                return BadRequest();
            }
            calcFunction.UpdateDate = DateTime.Now;
            calcFunction.User = HttpContext.Current.User.Identity.Name.ToString();
            db.Entry(calcFunction).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalcFunctionExists(id))
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
        public IHttpActionResult PostCalcFunctions(FunctionConfiguration calcFunction)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            calcFunction.UpdateDate = DateTime.Now;
            calcFunction.User = HttpContext.Current.User.Identity.Name.ToString();
            db.FunctionConfiguration.Add(calcFunction);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = calcFunction.ID }, calcFunction);
        }
        /// <summary>Posts new configuration on the database.
        /// <para>id = ID on DB Table </para>
        /// </summary>
        // DELETE: api/CalcConfigurations/5
        [ResponseType(typeof(CalcConfiguration))]
        public IHttpActionResult DeleteCalcFunction(int id)
        {
            FunctionConfiguration calcFunction = db.FunctionConfiguration.Find(id);
            if (calcFunction == null)
            {
                return NotFound();
            }
            db.FunctionConfiguration.Remove(calcFunction);
            db.SaveChanges();
            return Ok(calcFunction);
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

        private bool CalcFunctionExists(int id)
        {
            return db.FunctionConfiguration.Count(e => e.ID == id) > 0;
        }
    }
}