// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.IO;
using CalculationCSharp.Areas.Maintenance.Models;

namespace CalculationCSharp.Areas.Maintenance.Controllers
{
    public class MaintenanceController : Controller
    {
        // GET: Maintenance/Maintenance
        public ActionResult Index()
        {

            // Put all file names in root directory into array.
            string[] array1 = Directory.GetFiles(@HttpContext.Server.MapPath("\\Factor Tables\\"));

            List<SelectListItem> li = new List<SelectListItem>();
            int i = 0;

            // Display all files.
            Console.WriteLine("--- Files: ---");
            foreach (string name in array1)
            {
                string x = Convert.ToString(i);
                var pathParts = name.Split(Path.DirectorySeparatorChar);
                string fileName = pathParts.Last();
                li.Add(new SelectListItem { Text = fileName, Value = fileName });

                i += i;
            }
            ViewData["Dropdown"] = li;

            return View();
        }

        // GET: Maintenance/Maintenance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Maintenance/Maintenance/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DownloadFile(FormCollection value)
        {
            string filename = value["Dropdown"];
            string filepath = HttpContext.Server.MapPath("\\Factor Tables\\" + filename);
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        // POST: Maintenance/Maintenance/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(HttpContext.Server.MapPath("\\Factor Tables\\"), fileName);
                    file.SaveAs(path);
                }
                ViewBag.Message = "Upload successful";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Error");
            }
        }

        // GET: Maintenance/Maintenance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        // POST: Maintenance/Maintenance/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Maintenance/Maintenance/Delete/5
        public ActionResult DeleteFile(int id)
        {
            return View();
        }

        // POST: Maintenance/Maintenance/Delete/5
        [HttpPost]
        public ActionResult DeleteFile(FormCollection value)
        {
            try
            {
                // TODO: Add delete logic here
                string filename = value["Dropdown"];
                string filepath = HttpContext.Server.MapPath("\\Factor Tables\\" + filename);
                System.IO.File.Delete(filepath);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Delete failed - no file added to upload";
                return RedirectToAction("Error");
            }
        }
    }
}
