using System;
using System.Collections.Generic;
using CalculationCSharp.Areas.Config.Controllers;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Factors
    {
        public string TableName { get; set; }
        public string LookupType { get; set; }
        public dynamic LookupValue { get; set; }
        public string OutputType { get; set; }
        public bool RowMatch { get; set; }
        public string RowMatchLookupType { get; set; }
        public int RowMatchRowNo { get; set; }
        public string RowMatchValue { get; set; }
        public int ColumnNo { get; set; }
    }
}