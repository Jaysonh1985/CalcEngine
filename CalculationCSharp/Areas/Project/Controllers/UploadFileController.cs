using CalculationCSharp.Areas.Project.Models;
using CalculationCSharp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class UploadFileController : Controller
    {
        CalculationDBContext db = new CalculationDBContext();
        // GET: Project/UploadFile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveFiles(string description)
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            FileRepository FileRepo = new FileRepository();
            if (Request.Files != null)
            {
                var file = Request.Files[0];
                actualFileName = file.FileName;
                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                int size = file.ContentLength;
                
                try
                {
                    file.SaveAs(Path.Combine(Server.MapPath("\\Areas\\Project\\UploadedFiles\\"), fileName));
                    FileRepo.FileName = actualFileName;
                    FileRepo.FilePath = fileName;
                    FileRepo.FileSize = size;
                    FileRepo.Description = description;
                    db.FileRepository.Add(FileRepo);
                    db.SaveChanges();
                    Message = "file uploaded successfully";
                    flag = true;
                }
                catch (Exception)
                {
                    Message = "File upload failed! Please try again";
                }

            }
            return new JsonResult { Data = new { Message = Message, Status = flag, FileRepo = FileRepo } };
        }

        [HttpDelete]
        public JsonResult DeleteFiles(string FileName)
        {
            string Message;
            bool flag;
            Message =  string.Empty;
            flag = false;
            
            if (Request.Files != null)
            {
                FileRepository FileRepo = db.FileRepository.Where(i => i.FilePath == FileName).FirstOrDefault();
                try
                {
                    string filepath = HttpContext.Server.MapPath("\\Areas\\Project\\UploadedFiles\\" + FileName);
                    System.IO.File.Delete(filepath);
                    db.FileRepository.Remove(FileRepo);
                    db.SaveChanges();
                    Message = "file deleted successfully";
                    flag = true;
                }
                catch (Exception)
                {
                    Message = "File upload failed! Please try again";
                }
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

    }

}