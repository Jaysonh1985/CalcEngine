using CalculationCSharp.Models.ArrayFunctions;
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
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            MathsFunctions parameters = (MathsFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(MathsFunctions));

            List<string> D1parts = null;
            List<string> D2parts = null;
            string[] Numbers1parts = null;
            string[] Numbers2parts = null;

            D1parts = ArrayBuilder.InputArrayBuilder(parameters.Number1, jCategory, GroupID, ItemID);
            D2parts = ArrayBuilder.InputArrayBuilder(parameters.Number2, jCategory, GroupID, ItemID);

            if (D1parts != null)
            {
                Numbers1parts = D1parts.ToArray();
            }
            if (D2parts != null)
            {
                Numbers2parts = D2parts.ToArray();
            }

            string Output = null;
            Decimal OutputValue = 0;
            
            int MaxLength = ArrayBuilder.GetMaxLength(Numbers1parts, Numbers2parts);

            int Counter = 0;

            for (int i = 0; i < MaxLength; i++)
            {
                dynamic InputA = null;
                dynamic InputB = null;
                decimal InputADeci = 0;
                decimal InputBDeci = 0;

                if (Numbers1parts != null)
                {
                    InputA = Config.VariableReplace(jCategory, Numbers1parts[Counter], GroupID, ItemID);                   
                    decimal.TryParse(InputA, out InputADeci);
                }

                if(Numbers2parts != null)
                {
                    InputB = Config.VariableReplace(jCategory, Numbers2parts[Counter], GroupID, ItemID);
                    decimal.TryParse(InputB, out InputBDeci);
                }

                    if (InputA != null)
                    {
                            Decimal.TryParse(InputA, out InputADeci);
         
                    }
                    else
                    {
                        InputADeci = 0;
                    }
                    if (InputB != null)
                    {
                       
                        Decimal.TryParse(InputB, out InputBDeci);
                    }
                    else
                    {
                        InputBDeci = 0;
                    }

                    if (parameters.Type == "Abs")
                    {
                        OutputValue = MathFunctions.Abs(InputADeci);
                    }
                    else if (parameters.Type == "Ceiling")
                    {
                        OutputValue = MathFunctions.Ceiling(InputADeci);
                    }
                    else if (parameters.Type == "Floor")
                    {
                        OutputValue = MathFunctions.Floor(InputADeci);
                    }
                    else if (parameters.Type == "Max")
                    {
                        OutputValue = MathFunctions.Max(InputADeci, InputBDeci);
                    }
                    else if (parameters.Type == "Min")
                    {
                        OutputValue = MathFunctions.Min(InputADeci, InputBDeci);
                    }
                    else if (parameters.Type == "Truncate")
                    {
                        OutputValue = MathFunctions.Truncate(InputADeci);
                    }

                    Output = Output + OutputValue + ",";
                    Counter = Counter + 1;

            }
                

            Output = Output.Remove(Output.Length - 1);
            return Convert.ToString(Output);
        }

    }
}