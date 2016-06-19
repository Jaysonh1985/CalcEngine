using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models
{
    public class Stories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Moscow { get; set; }
        public string User { get; set; }
        public string Timebox { get; set; }
        public string AcceptanceCriteria { get; set; }
        public virtual List<Comments> Comments { get; set; }
        public int ColumnId { get; set; }
        public virtual List<Tasks> Tasks { get; set; }
    }
}