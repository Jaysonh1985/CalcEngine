using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Function
    {
        public int ID { get; set; }
        public string Scheme { get; set; }
        public string FunctionName { get; set; }
        public List<ConfigViewModel> Input { get; set; }
    }
}