// Copyright (c) 2016 Project AIM
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Project.Models
{
    public class ColumnRepository
    {

        public List<Column> GetColumns(ProjectBoard ProjectBoard)
        {
            if (ProjectBoard.Configuration == "" || ProjectBoard.Configuration == null || ProjectBoard == null)
            {
                var columns = new List<Column>();
                var stories = new List<Stories>();
                columns.Add(new Column { Description = "Backlog Column", Id = 1, Name = "Backlog", Stories = stories });
                columns.Add(new Column { Description = "In Progress Column", Id = 2, Name = "In Progress", Stories = new List<Stories>() });
                columns.Add(new Column { Description = "Pending Column", Id = 3, Name = "Pending", Stories = new List<Stories>() });
                columns.Add(new Column { Description = "Release Column", Id = 4, Name = "Release", Stories = new List<Stories>() });
                return columns;
            }
            else
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string jsonString = ProjectBoard.Configuration;
                List<Column> columns = (List<Column>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<Column>));
                return columns;
            }              
        }
    }
}