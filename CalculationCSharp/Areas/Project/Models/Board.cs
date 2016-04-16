using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Column> Configuration { get; set; }
    }
}