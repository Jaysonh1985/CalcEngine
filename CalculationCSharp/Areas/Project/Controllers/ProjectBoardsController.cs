using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CalculationCSharp.Models;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class ProjectBoardsController : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: Project/ProjectBoards
        public ActionResult Index()
        {
            return View(db.ProjectBoard.ToList());
        }

        // GET: Project/ProjectBoards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectBoard projectBoard = db.ProjectBoard.Find(id);
            if (projectBoard == null)
            {
                return HttpNotFound();
            }
            return View(projectBoard);
        }

        // GET: Project/ProjectBoards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/ProjectBoards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Group,Name,User,Configuration,UpdateDate")] ProjectBoard projectBoard)
        {
            if (ModelState.IsValid)
            {
                db.ProjectBoard.Add(projectBoard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectBoard);
        }

        // GET: Project/ProjectBoards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectBoard projectBoard = db.ProjectBoard.Find(id);
            if (projectBoard == null)
            {
                return HttpNotFound();
            }
            return View(projectBoard);
        }

        // POST: Project/ProjectBoards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Group,Name,User,Configuration,UpdateDate")] ProjectBoard projectBoard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectBoard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectBoard);
        }

        // GET: Project/ProjectBoards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectBoard projectBoard = db.ProjectBoard.Find(id);
            if (projectBoard == null)
            {
                return HttpNotFound();
            }
            return View(projectBoard);
        }

        // POST: Project/ProjectBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectBoard projectBoard = db.ProjectBoard.Find(id);
            db.ProjectBoard.Remove(projectBoard);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
