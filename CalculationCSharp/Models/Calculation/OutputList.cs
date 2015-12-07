using System.IO;
using System;
using Microsoft.Office.Interop;
using System.Web.Mvc;
using System.Collections.Generic;
using CalculationCSharp.Models.Calculation;
using System.ComponentModel.DataAnnotations;

namespace CalculationCSharp.Models.Calculation
{
    public class OutputList
    {
        public string ID { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }

        public void ListBuild(List<OutputList> List, string ID, string Field, object Value1, string Group)
        {
            var Value = Convert.ToString(Value1);

            List.Add(new OutputList
            {
                ID = ID,
                Field = Field,
                Value = Value,
                Group = Group
            });

        }
    }

}
