using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public double RemainingTime { get; set; }
        public bool Complete { get; set; }              
    }
}