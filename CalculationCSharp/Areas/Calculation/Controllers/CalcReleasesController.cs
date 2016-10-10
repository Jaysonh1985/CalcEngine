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

namespace CalculationCSharp.Areas.Calculation.Controllers
{
    public class CalcReleasesController : ApiController
    {
        /// <summary>Controller for updating the calculation releases table and displays this on the Index page.
        /// </summary>

        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcReleases
        public IQueryable<CalcRelease> GetCalcRelease()
        {
            return db.CalcRelease;
        }
        /// <summary>Get list of Calcs available in the Configuration System.
        /// <para>id = CalculationID on DB Table </para>
        /// </summary>
        // GET: api/CalcReleases/5
        [ResponseType(typeof(CalcRelease))]
        public IHttpActionResult GetCalcRelease(int id)
        {
            var List = db.CalcRelease.Where(i => i.CalcID == id);
            return Ok(List);
        }
        /// <summary>Put calculation in CalcReleases Table.
        /// <para>id = CalculationID on DB Table</para>
        /// <para>calcRelease = Configuration to update</para>
        /// </summary>
        // PUT: api/CalcReleases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalcRelease(int id, CalcRelease calcRelease)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CalcRelease CalcReleaseFind = db.CalcRelease.FirstOrDefault(i => i.CalcID == id);
            CalcReleaseFind.User = HttpContext.Current.User.Identity.Name.ToString();
            CalcReleaseFind.UpdateDate = DateTime.Now;
            CalcReleaseFind.Configuration = calcRelease.Configuration;
            CalcReleaseFind.Version = calcRelease.Version;
            db.Entry(CalcReleaseFind).State = EntityState.Modified;
            
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


        /// <summary>Post calculation in CalcReleases Table.
        /// <para>calcRelease = Configuration to update</para>
        /// </summary>

        // POST: api/CalcReleases
        [ResponseType(typeof(CalcRelease))]
        public IHttpActionResult PostCalcRelease(CalcRelease calcRelease)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            calcRelease.CalcID = calcRelease.ID;
            calcRelease.Configuration = Convert.ToString(calcRelease.Configuration);
            calcRelease.User = HttpContext.Current.User.Identity.Name.ToString();
            calcRelease.UpdateDate = DateTime.Now;

            db.CalcRelease.Add(calcRelease);
            db.SaveChanges();

            return Ok(calcRelease);
        }

        /// <summary>Delete calculation in CalcReleases Table.
        /// <para>id = calculation ID in Table</para>
        /// </summary>
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