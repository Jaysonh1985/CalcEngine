using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class ResultViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ExpectedResult { get; set; }
        public virtual List<FunctionViewModel> Functions { get; set; }
        public bool Expanded { get; set; }
    }
}