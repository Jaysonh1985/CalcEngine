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
        /// <summary>Controller for updating the calculation histories table and displays this on the Histories Modal.
        /// </summary>
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/CalcHistories
        public IQueryable<CalcHistory> GetCalcHistory()
        {
            return db.CalcHistory;
        }

        /// <summary>Get list of Calc Histories available in the Configuration System.
        /// <para>id = CalculationID on DB Table </para>
        /// </summary>
        
        // GET: api/CalcHistories/5
        [ResponseType(typeof(CalcHistory))]
        public IHttpActionResult GetCalcHistory(int id, bool SelectList)
        {
            if(SelectList == true)
            {
                var List = db.CalcHistory.Where(i => i.CalcID == id);
                return Ok(List);
            }
            else
            {
                CalcHistory calcHistory = db.CalcHistory.Find(id);
                if (calcHistory == null)
                {
                    return NotFound();
                }
                return Ok(calcHistory);
            }
        }

        /// <summary>Put calculation in CalcHistories Table.
        /// <para>id = CalculationID on DB Table</para>
        /// <para>calcHistory = Calc History object to save to DB</para>
        /// </summary>
        
        // PUT: api/CalcHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalcHistory(int id, CalcHistory calcHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        /// <summary>Post calculation in CalcHistories Table.
        /// <para>calcRelease = Configuration to update</para>
        /// </summary>

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

        /// <summary>Delete calculation in CalcHistories Table.
        /// <para>id = calculation ID in Table</para>
        /// </summary>
        /// 
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