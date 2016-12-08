// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using CalculationCSharp.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using CalculationCSharp.Areas.Project.Models;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Xml;
using System.Text;

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

            return View();
        }

        [HttpGet]
        public ActionResult Board(int? id)
        {

            ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(id));

            if(ProjectBoard == null)
            {
                ViewData["H1"] = "New Board";
            }
            else
            {
                ViewData["H1"] = ProjectBoard.Name;

            }

            return View();

        }


        [HttpGet]
        public FileContentResult DumpToCSV(int? id)
        {
            ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(id));

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = Convert.ToString(ProjectBoard.Configuration);
            
            List<Column> columns = (List<Column>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<Column>));

            StringBuilder sb = new StringBuilder();
            List<string> propNames;
            List<string> propValues;
            bool isNameDone = false;

            //Get property collection and set selected property list
            PropertyInfo[] props = typeof(Column).GetProperties();
            List<PropertyInfo> propList = GetSelectedProperties(props, "", "");
            //Iterate through data list collection
            foreach (var item in columns)
            {
                sb.AppendLine("");
                propNames = new List<string>();
                propValues = new List<string>();

                //Iterate through property collection
                foreach (var prop in propList)
                {
                    if (!isNameDone) propNames.Add(prop.Name);
                                       
                }
                //Add line for Names
                string line = string.Empty;
                if (!isNameDone)
                {
                    line = string.Join(",", propNames);
                    sb.AppendLine(line);
                    sb.AppendLine("");
                    isNameDone = true;
                }
                foreach (var col in columns)
                {
                    foreach(var story in col.Stories)
                    {
                        
                        propValues.Add(Convert.ToString(story.Id));
                        propValues.Add(story.Name);
                        propValues.Add(story.Description);
                        propValues.Add(story.Moscow);
                        propValues.Add(col.Name);
                        propValues.Add(story.User);
                        line = string.Join(",", propValues);
                        sb.Append(line);
                        sb.AppendLine("");
                    }
                }
                //Add line for the values             
                
            }

            var csv = Convert.ToString(sb);
            csv = csv.Replace("\r\n", string.Empty);

            return File(new System.Text.UTF8Encoding().GetBytes("1"), "text/csv", "Custom Report.csv");

        }

        private static List<PropertyInfo> GetSelectedProperties(PropertyInfo[] props, string include, string exclude)
        {
            List<PropertyInfo> propList = new List<PropertyInfo>();
            if (include != "") //Do include first
            {
                var includeProps = include.ToLower().Split(',').ToList();
                foreach (var item in props)
                {
                    var propName = includeProps.Where(a => a == item.Name.ToLower()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(propName))
                        propList.Add(item);
                }
            }
            else if (exclude != "") //Then do exclude
            {
                var excludeProps = exclude.ToLower().Split(',');
                foreach (var item in props)
                {
                    var propName = excludeProps.Where(a => a == item.Name.ToLower()).FirstOrDefault();
                    if (string.IsNullOrEmpty(propName))
                        propList.Add(item);
                }
            }
            else //Default
            {
                propList.AddRange(props.ToList());
            }
            return propList;
        }
    }
}