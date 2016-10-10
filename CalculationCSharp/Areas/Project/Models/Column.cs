// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models
{
    public partial class Column
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<Stories> Stories { get; set; }
    }
}