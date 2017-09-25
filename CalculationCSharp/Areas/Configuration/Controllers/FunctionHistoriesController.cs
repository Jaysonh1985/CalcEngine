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
    public class FunctionHistoriesController : ApiController
    {
        /// <summary>Controller for updating the calculation histories table and displays this on the Histories Modal.
        /// </summary>
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/FunctionHistory
        public IQueryable<FunctionHistory> GetFunctionHistory()
        {
            return db.FunctionHistory;
        }

        /// <summary>Get list of Calc Histories available in the Configuration System.
        /// <para>id = CalculationID on DB Table </para>
        /// </summary>

        // GET: api/FunctionHistory/5
        [ResponseType(typeof(FunctionHistory))]
        public IHttpActionResult GetFunctionHistory(int id, bool SelectList)
        {
            if(SelectList == true)
            {
                var List = db.FunctionHistory.Where(i => i.CalcID == id);
                return Ok(List);
            }
            else
            {
                FunctionHistory FunctionHistory = db.FunctionHistory.Find(id);
                if (FunctionHistory == null)
                {
                    return NotFound();
                }
                return Ok(FunctionHistory);
            }
        }

        /// <summary>Put calculation in CalcHistories Table.
        /// <para>id = CalculationID on DB Table</para>
        /// <para>FunctionHistory = Calc History object to save to DB</para>
        /// </summary>

        // PUT: api/CalcHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFunctionHistory(int id, FunctionHistory FunctionHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FunctionHistory.UpdateDate = DateTime.Now;
            db.Entry(FunctionHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FunctionHistoryExists(id))
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
        [ResponseType(typeof(FunctionHistory))]
        public IHttpActionResult PostFunctionHistory(FunctionHistory FunctionHistory)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            FunctionHistory.UpdateDate = DateTime.Now;
            db.FunctionHistory.Add(FunctionHistory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = FunctionHistory.ID }, FunctionHistory);
        }

        /// <summary>Delete calculation in CalcHistories Table.
        /// <para>id = calculation ID in Table</para>
        /// </summary>
        /// 
        // DELETE: api/CalcHistories/5
        [ResponseType(typeof(FunctionHistory))]
        public IHttpActionResult DeleteFunctionHistory(int id)
        {
            FunctionHistory FunctionHistory = db.FunctionHistory.Find(id);
            if (FunctionHistory == null)
            {
                return NotFound();
            }

            db.FunctionHistory.Remove(FunctionHistory);
            db.SaveChanges();

            return Ok(FunctionHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FunctionHistoryExists(int id)
        {
            return db.FunctionHistory.Count(e => e.ID == id) > 0;
        }
    }
}