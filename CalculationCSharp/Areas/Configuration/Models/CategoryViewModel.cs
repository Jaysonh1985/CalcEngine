﻿using System.Collections.Generic;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class CategoryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<ConfigViewModel> Functions { get; set; }
    }
}