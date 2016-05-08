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
        public dynamic Input1 { get; set; }
        public dynamic Input2 { get; set; }
        public string Logic { get; set; }
        public double Rounding { get; set; }
        public string RoundingType { get; set; }

        public double Setup (List<ConfigViewModel> ConfigViewModel, Maths parameters, int ID)
        {
            CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

            Input1 = Config.VariableReplace(ConfigViewModel, parameters.Input1, ID);
            Input2 = Config.VariableReplace(ConfigViewModel, parameters.Input2, ID);
            Logic = parameters.Logic;
            Rounding = parameters.Rounding;
            RoundingType = parameters.RoundingType;

            Input1 = Convert.ToDouble(Input1);
            Input2 = Convert.ToDouble(Input2);
            double total = Input1 + Input2;
            return total;
        }
    }
}