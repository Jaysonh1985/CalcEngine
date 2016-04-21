using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Project.Models
{
    public class BoardRepository
    {
        CalculationDBContext db = new CalculationDBContext();
        ProjectBoard ProjectBoard = new ProjectBoard();

        public List<ProjectBoard> GetBoards()
        {
            var ProjectBoard = db.ProjectBoard.ToList();

            return ProjectBoard;
        }

        public ProjectBoard GetBoard(dynamic json)
        {
            if(json.boardId == null)
            {
                return null;
            }
            else
            {
                ProjectBoard Board  = db.ProjectBoard.Find(Convert.ToInt32(json.boardId));
                return Board;
            }
                        
        }

        public void AddBoard(dynamic json)
        {
            ProjectBoard.Name = json.boardName;
            ProjectBoard.Configuration = Convert.ToString(json.data);
            ProjectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
            ProjectBoard.Group = "Project Group";
            ProjectBoard.UpdateDate = DateTime.Now;
            db.ProjectBoard.Add(ProjectBoard);
            db.SaveChanges();
        }

        public void UpdateBoard(dynamic json)
        {
            ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(json.boardId));
            ProjectBoard.Name = json.boardName;
            ProjectBoard.Configuration = Convert.ToString(json.data);
            ProjectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
            ProjectBoard.Group = "Project Group";
            ProjectBoard.UpdateDate = DateTime.Now;

            db.Entry(ProjectBoard).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void DeleteBoard(dynamic json)
        {
            var db = new CalculationDBContext();
            ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(json.boardId));
            db.ProjectBoard.Remove(ProjectBoard);
            db.SaveChanges();
        }

    }
}