// Copyright (c) 2016 Project AIM
using CalculationCSharp.Models.ArrayFunctions;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Table
    {
        public dynamic ColumnName { get; set; }
        public dynamic Variable { get; set; }
        public dynamic Result { get; set; }
        public dynamic SummaryVariable { get; set; }
        public dynamic SummaryResult { get; set; }

    }
}