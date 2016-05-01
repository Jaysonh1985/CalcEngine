using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Add
    {
        public Double Input1 { get; set; }
        public Double Input2 { get; set; }

        public Double AddFunction (double Input1, double Input2)
        {
            double total = Input1 + Input2;
            return Input1;
        }
    }
}