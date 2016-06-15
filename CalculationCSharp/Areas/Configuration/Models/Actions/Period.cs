using System;
using System.Collections.Generic;
using CalculationCSharp.Areas.Config.Controllers;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Period
    {
        public string DateAdjustmentType { get; set; }
        public dynamic Date1 { get; set; }
        public dynamic Date2 { get; set; }
        public bool Inclusive { get; set; }
        public double DaysinYear { get; set; }
    }
}