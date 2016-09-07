using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class MathsFunctions
    {
        public string Type { get; set; }
        public dynamic Number1 { get; set; }
        public dynamic Number2 { get; set; }

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output (string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            MathematicalFunctions MathFunctions = new MathematicalFunctions();
            MathsFunctions parameters = (MathsFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(MathsFunctions));
            dynamic InputA = Config.VariableReplace(jCategory, parameters.Number1, GroupID, ItemID);
            dynamic InputB = Config.VariableReplace(jCategory, parameters.Number2, GroupID, ItemID);
            decimal InputADeci;
            decimal InputBDeci;
            decimal.TryParse(InputA, out InputADeci);
            decimal.TryParse(InputB, out InputBDeci);
            Decimal Output;

            Output = 0;

            if (parameters.Type == "Abs")
            {
                Output = MathFunctions.Abs(InputADeci);
            }
            else if (parameters.Type == "Add")
            {
                Output = MathFunctions.Add(InputADeci, InputBDeci);
            }
            else if (parameters.Type == "Ceiling")
            {
                Output = MathFunctions.Ceiling(InputADeci);
            }
            else if (parameters.Type == "Divide")
            {
                Output = MathFunctions.Divide(InputADeci, InputBDeci);
            }
            else if (parameters.Type == "Floor")
            {
                Output = MathFunctions.Floor(InputADeci);
            }
            else if (parameters.Type == "Max")
            {
                Output = MathFunctions.Max(InputADeci, InputBDeci);
            }
            else if (parameters.Type == "Min")
            {
                Output = MathFunctions.Min(InputADeci, InputBDeci);
            }
            else if (parameters.Type == "Multiply")
            {
                Output = MathFunctions.Multiply(InputADeci, InputBDeci);
            }
            else if (parameters.Type == "Subtract")
            {
                Output = MathFunctions.Subtract(InputADeci, InputBDeci);
            }

            else if (parameters.Type == "Truncate")
            {
                Output = MathFunctions.Truncate(InputADeci);
            }
            return Convert.ToString(Output);
        }

    }
}