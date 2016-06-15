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
        public dynamic LookupValue { get; set; }
        public int DataType { get; set; }
        public int ColumnNo { get; set; }
    }
}