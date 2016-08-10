using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models
{
    public class Tasks
    {

        public string TaskName { get; set; }
        public string TaskUser { get; set; }
        public string RemainingTime { get; set; }
        public string Status { get; set; }              
    }
}