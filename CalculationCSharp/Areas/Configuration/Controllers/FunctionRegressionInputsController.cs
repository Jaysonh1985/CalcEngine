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
    public class FunctionRegressionInputsController : ApiController
    {
        /// <summary>Controller for getting the Calculation Reg.
        /// </summary>
        private CalculationDBContext db = new CalculationDBContext();

        // GET: api/FunctionRegressionInputs
        public IQueryable<FunctionRegressionInputs> GetFunctionRegressionInputs()
        {
            return db.FunctionRegressionInputs;
        }
        /// <summary>Get list of Calc Regressions Members available in the for the configuration working on.
        /// <para>id = ID on DB Table </para>
        /// </summary>
        // GET: api/FunctionRegressionInputs/5
        [ResponseType(typeof(FunctionRegressionInputs))]
        public IHttpActionResult GetFunctionRegressionInputs(int id)
        {
            var List = db.FunctionRegressionInputs.Where(i => i.CalcID == id);

            return Ok(List);
        }
        /// <summary>Put calculation and member inputs into in calcRegression Table.
        /// <para>id = CalculationID on DB Table</para>
        /// <para>FunctionRegressionInputs = Calc Regression Object to be applied to DB</para>
        /// </summary>
        // PUT: api/FunctionRegressionInputs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFunctionRegressionInputs(int id, FunctionRegressionInputs FunctionRegressionInputs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != FunctionRegressionInputs.ID)
            {
                return BadRequest();
            }

            db.Entry(FunctionRegressionInputs).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FunctionRegressionInputsExists(id))
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
        /// <summary>Post calculation and member inputs into in calcRegression Table.
        /// <para>FunctionRegressionInputs = Calc Regression Object to be applied to DB</para>
        /// </summary>
        // POST: api/FunctionRegressionInputs
        [ResponseType(typeof(FunctionRegressionInputs))]
        public IHttpActionResult PostFunctionRegressionInputs(FunctionRegressionInputs FunctionRegressionInputs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FunctionConfiguration calcConfiguration = db.FunctionConfiguration.Find(FunctionRegressionInputs.CalcID);
            FunctionRegressionInputs.Scheme = calcConfiguration.Scheme;
            FunctionRegressionInputs.Type = calcConfiguration.Name;

            db.FunctionRegressionInputs.Add(FunctionRegressionInputs);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = FunctionRegressionInputs.ID }, FunctionRegressionInputs);
        }
        /// <summary>Delete calculation and member inputs from calcRegression Table.
        /// <para>id = CalculationID on DB Table</para>
        /// </summary>
        // DELETE: api/FunctionRegressionInputs/5
        [ResponseType(typeof(FunctionRegressionInputs))]
        public IHttpActionResult DeleteFunctionRegressionInputs(int id)
        {
            FunctionRegressionInputs FunctionRegressionInputs = db.FunctionRegressionInputs.Find(id);
            if (FunctionRegressionInputs == null)
            {
                return NotFound();
            }

            db.FunctionRegressionInputs.Remove(FunctionRegressionInputs);
            db.SaveChanges();

            return Ok(FunctionRegressionInputs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FunctionRegressionInputsExists(int id)
        {
            return db.FunctionRegressionInputs.Count(e => e.ID == id) > 0;
        }
    }
}