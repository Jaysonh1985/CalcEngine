using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using CalculationCSharp.Models;
using System.Net;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class BoardController : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: Project/ProjectBoards
        public ActionResult Index()
        {

            ViewData["H1"] = "Project Management";
            ViewData["P1"] = "";

            return View(db.ProjectBoard.ToList());
        }

        [HttpGet]
        public ActionResult Board()
        {
            ViewData["H1"] = "Project Board";
            ViewData["P1"] = "";
            return View();
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
            db.ProjectBoard.Remove(projectBoard);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}