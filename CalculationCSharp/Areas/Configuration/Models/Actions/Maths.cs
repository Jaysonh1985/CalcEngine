// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using CalculationCSharp.Areas.Config.Controllers;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Maths
    {
        public string Bracket1 { get; set; }
        public dynamic Input1 { get; set; }
        public dynamic Input2 { get; set; }
        public string Logic { get; set; }
        public int? Rounding { get; set; }
        public string RoundingType { get; set; }
        public string Bracket2 { get; set; }
        public string Logic2 { get; set; }

    }
}