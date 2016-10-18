// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using CalculationCSharp.Areas.Config.Controllers;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Web.Script.Serialization;

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

        /// <summary>Outputs where Maths Function has been called.
        /// <para>jparameters = specific parameters for the model</para>
        /// <para>jCategory = full configuration</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// <para>MathString = Iterative string builder</para>
        /// <para>PowOpen = Power logic for the calculation</para>
        /// </summary>
        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        public String Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID, string MathString, bool PowOpen)
        {
            CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
            string formula = null;
            Maths Maths = new Maths();
            Maths parameters = (Maths)javaScriptSerializ­er.Deserialize(jparameters, typeof(Maths));
            dynamic InputA = Config.VariableReplace(jCategory, parameters.Input1, GroupID, ItemID);
            dynamic InputB = Config.VariableReplace(jCategory, parameters.Input2, GroupID, ItemID);
            string Bracket1 = Convert.ToString(parameters.Bracket1);
            string Input1 = Convert.ToString(InputA);
            string Logic = Convert.ToString(parameters.Logic);
            string Input2 = Convert.ToString(InputB);
            string Bracket2 = Convert.ToString(parameters.Bracket2);
            string Rounding = Convert.ToString(parameters.Rounding);
            string RoundingType = Convert.ToString(parameters.RoundingType);
            string Logic2 = Convert.ToString(parameters.Logic2);

            //TODO bug with the way the rounding calculation works in that if the dropdown is blank it populates with 0 when it should populate with nil, we want the rounding to default to 2
            if (Rounding == "0")
            {
                Rounding = "2";
            }
            if (Rounding == "10")
            {
                Rounding = "0";
            }
            if (Logic == "Pow")
            {
                formula = Logic + '(' + Input1 + ',' + Input2 + ')';
            }
            else
            {
                formula = Input1 + Logic + Input2;
            }
            string MathString1;
            if (Logic2 == "Pow")
            {
                MathString1 = string.Concat(Logic2, "(", MathString, Bracket1, formula, Bracket2, ",");
            }
            else
            {
                MathString1 = string.Concat(MathString, Bracket1, formula, Bracket2, Logic2);
            }
            if (Logic2 != "Pow" && PowOpen == true)
            {
                MathString1 = string.Concat(MathString1, ")");
            }
            return MathString1;
        }
        /// <summary>PowOpen function checks if the math string has a power variable contrinaed with in this is required because the power formula is structured differently.
        /// <para>jparameters = specific parameters for the model</para>
        /// <para>PowOpen = Power logic for the calculation</para>
        /// </summary>
        public bool PowOpen(string jparameters,  bool PowOpen)
        {
            Maths parameters = (Maths)javaScriptSerializ­er.Deserialize(jparameters, typeof(Maths));
            string Logic = Convert.ToString(parameters.Logic);
            if (Logic2 == "Pow")
            {
                return true;
            }
            else if (Logic2 != "Pow" && PowOpen == true)
            {              
               return false;
            }
            else
            {
                return false;
            }
        }
    }
}