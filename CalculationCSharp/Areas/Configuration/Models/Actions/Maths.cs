using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Maths
    {
        public double Input1 { get; set; }
        public string Logic { get; set; }
        public double Input2 { get; set; }
        public double Rounding { get; set; }
        public string RoundingType { get; set; }

        public Double AddFunction (double Input1, double Input2)
        {
            double total = Input1 + Input2;
            return Input1;
        }
    }
}