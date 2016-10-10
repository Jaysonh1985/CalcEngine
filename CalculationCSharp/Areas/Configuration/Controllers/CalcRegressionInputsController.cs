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

namespace CalculationCSharp.Areas.Configuration.Controllers
{
    public class CalcRegressionInputsController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcRegressionInputs
        public IQueryable<CalcRegressionInputs> GetCalcRegressionInputs()
        {
            return db.CalcRegressionInputs;
        }

        // GET: api/CalcRegressionInputs/5
        [ResponseType(typeof(CalcRegressionInputs))]
        public IHttpActionResult GetCalcRegressionInputs(int id)
        {
            var List = db.CalcRegressionInputs.Where(i => i.CalcID == id);

            return Ok(List);
        }

        // PUT: api/CalcRegressionInputs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalcRegressionInputs(int id, CalcRegressionInputs calcRegressionInputs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calcRegressionInputs.ID)
            {
                return BadRequest();
            }

            db.Entry(calcRegressionInputs).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalcRegressionInputsExists(id))
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

        // POST: api/CalcRegressionInputs
        [ResponseType(typeof(CalcRegressionInputs))]
        public IHttpActionResult PostCalcRegressionInputs(CalcRegressionInputs calcRegressionInputs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CalcRegressionInputs.Add(calcRegressionInputs);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = calcRegressionInputs.ID }, calcRegressionInputs);
        }

        // DELETE: api/CalcRegressionInputs/5
        [ResponseType(typeof(CalcRegressionInputs))]
        public IHttpActionResult DeleteCalcRegressionInputs(int id)
        {
            CalcRegressionInputs calcRegressionInputs = db.CalcRegressionInputs.Find(id);
            if (calcRegressionInputs == null)
            {
                return NotFound();
            }

            db.CalcRegressionInputs.Remove(calcRegressionInputs);
            db.SaveChanges();

            return Ok(calcRegressionInputs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalcRegressionInputsExists(int id)
        {
            return db.CalcRegressionInputs.Count(e => e.ID == id) > 0;
        }
    }
}