using System;
using System.Collections.Generic;
using CalculationCSharp.Areas.Config.Controllers;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class MathsFunctions
    {
        public string Type { get; set; }
        public dynamic Number1 { get; set; }
        public dynamic Number2 { get; set; }

    }
}