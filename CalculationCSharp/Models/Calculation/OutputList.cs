// Copyright (c) 2016 Project AIM
using System.IO;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using CalculationCSharp.Models.Calculation;
using System.ComponentModel.DataAnnotations;
using CalculationCSharp.Areas.Configuration.Models;

namespace CalculationCSharp.Models.Calculation
{
    public class OutputList
    {
        public string ID { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public virtual List<OutputListGroup> SubOutput { get; set; }

    }

    public class OutputListGroup
    {
        public int ID { get; set; }
        public string Group { get; set; }
        public List<OutputList> Output { get; set; }
    }
}
