using System;
using System.Collections.Generic;
using CalculationCSharp.Areas.Config.Controllers;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Dates
    {
        public string Type { get; set; }
        public dynamic Date1 { get; set; }
        public dynamic Date2 { get; set; }
        public string PeriodType { get; set; }
        public dynamic Period { get; set; }
        public string Adjustment { get; set; }
        public string Day { get; set; }
    }
}