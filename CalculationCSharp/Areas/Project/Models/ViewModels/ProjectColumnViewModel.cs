using CalculationCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models.ViewModels
{
    public class ProjectColumnViewModel
    {
        public int ColumnId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProjectStories> ProjectStories { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}