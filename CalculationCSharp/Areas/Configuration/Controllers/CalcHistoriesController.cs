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
    public class CalcHistoriesController : ApiController
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcHistories
        public IQueryable<CalcHistory> GetCalcHistory()
        {
            return db.CalcHistory;
        }

        // GET: api/CalcHistories/5
        [ResponseType(typeof(CalcHistory))]
        public IHttpActionResult GetCalcHistory(int id)
        {
            var List = db.CalcHistory.Where(i => i.CalcID == id);

            return Ok(List);
        }

        // PUT: api/CalcHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalcHistory(int id, CalcHistory calcHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != calcHistory.ID)
            //{
            //    return BadRequest();
            //}

            calcHistory.UpdateDate = DateTime.Now;

            db.Entry(calcHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalcHistoryExists(id))
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

        // POST: api/CalcHistories
        [ResponseType(typeof(CalcHistory))]
        public IHttpActionResult PostCalcHistory(CalcHistory calcHistory)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            calcHistory.UpdateDate = DateTime.Now;
            db.CalcHistory.Add(calcHistory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = calcHistory.ID }, calcHistory);
        }

        // DELETE: api/CalcHistories/5
        [ResponseType(typeof(CalcHistory))]
        public IHttpActionResult DeleteCalcHistory(int id)
        {
            CalcHistory calcHistory = db.CalcHistory.Find(id);
            if (calcHistory == null)
            {
                return NotFound();
            }

            db.CalcHistory.Remove(calcHistory);
            db.SaveChanges();

            return Ok(calcHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalcHistoryExists(int id)
        {
            return db.CalcHistory.Count(e => e.ID == id) > 0;
        }
    }
}