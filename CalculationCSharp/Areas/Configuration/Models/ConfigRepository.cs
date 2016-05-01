using CalculationCSharp.Areas.Configuration.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ConfigRepository
    {
        Config Configuration = new Config();

        public List<Config> GetConfig(Config Config)
        {
        
            if (HttpContext.Current.Cache["configuration"] == null)
            {
                if (Config == null)
                {
                    var Configuration = new List<Config>();

                    Configuration.Add(new Config { ID = "1", Name = "Service", Category = "Service", Function = "Add", Output = "Start", Parameter = "", Type = "Type" });

                    HttpContext.Current.Cache["configuration"] = Configuration;
                }
                else
                { 

                }

            }
            return (List<Config>)HttpContext.Current.Cache["configuration"];
        }
    }

        //public ProjectBoard GetBoard(dynamic json)
        //{
        //    if(json.boardId == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        ProjectBoard Board  = db.ProjectBoard.Find(Convert.ToInt32(json.boardId));
        //        return Board;
        //    }
                        
        //}

        //public void AddBoard(dynamic json)
        //{
        //    ProjectBoard.Name = json.boardName;
        //    ProjectBoard.Configuration = Convert.ToString(json.data);
        //    ProjectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
        //    ProjectBoard.Group = "Project Group";
        //    ProjectBoard.UpdateDate = DateTime.Now;
        //    db.ProjectBoard.Add(ProjectBoard);
        //    db.SaveChanges();
        //}

        //public void UpdateBoard(dynamic json)
        //{
        //    ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(json.boardId));
        //    ProjectBoard.Name = json.boardName;
        //    ProjectBoard.Configuration = Convert.ToString(json.data);
        //    ProjectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
        //    ProjectBoard.Group = "Project Group";
        //    ProjectBoard.UpdateDate = DateTime.Now;

        //    db.Entry(ProjectBoard).State = EntityState.Modified;

        //    db.SaveChanges();
        //}

        //public void DeleteBoard(dynamic json)
        //{
        //    var db = new CalculationDBContext();
        //    ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(json.boardId));
        //    db.ProjectBoard.Remove(ProjectBoard);
        //    db.SaveChanges();
        //}

    //}
}