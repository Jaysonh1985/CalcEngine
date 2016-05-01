using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Config
    {
        public string ID { get; set; }
        public string Type {get;set;}
        public string Category { get; set; }
        public string Function { get; set; }
        public string Name { get; set; }
        public string Parameter { get; set; }
        public string Output { get; set; }

    }
}