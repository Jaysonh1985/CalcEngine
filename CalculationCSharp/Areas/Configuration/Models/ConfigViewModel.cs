using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ConfigViewModel
    {
        public int ID { get; set; }
        public string Type {get;set;}
        public string Category { get; set; }
        public string Function { get; set; }
        public string Name { get; set; }
        public virtual List<dynamic> Parameter { get; set; }
        public string Output { get; set; }

    }
}