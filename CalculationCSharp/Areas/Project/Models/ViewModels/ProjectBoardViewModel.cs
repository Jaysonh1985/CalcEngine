using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models.ViewModels
{
    public class ProjectBoardViewModel
    {
        public int BoardId { get; set; }
        public string Client { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}