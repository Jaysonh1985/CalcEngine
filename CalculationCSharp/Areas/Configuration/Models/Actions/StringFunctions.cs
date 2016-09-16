using CalculationCSharp.Models.ArrayFunctions;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class StringFunctions
    {
        public string Type { get; set; }
        public dynamic String1 { get; set; }
        public dynamic Number1 { get; set; }
        public dynamic String2 { get; set; }

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output (string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            MathematicalFunctions MathFunctions = new MathematicalFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            StringFunctions parameters = (StringFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(StringFunctions));

            List<string> D1parts = null;
            List<string> D2parts = null;
            string[] Numbers1parts = null;
            string[] Numbers2parts = null;

            D1parts = ArrayBuilder.InputArrayBuilder(parameters.String1, jCategory, GroupID, ItemID);
            D2parts = ArrayBuilder.InputArrayBuilder(parameters.Number1, jCategory, GroupID, ItemID);

            if (D1parts != null)
            {
                Numbers1parts = D1parts.ToArray();
            }
            if (D2parts != null)
            {
                Numbers2parts = D2parts.ToArray();
            }

            string Output = null;
            string OutputValue = null;
            int MaxLength = ArrayBuilder.GetMaxLength(Numbers1parts, Numbers2parts);

            int Counter = 0;

            for (int i = 0; i < MaxLength; i++)
            {
                dynamic InputA = null;
                string InputAString = null;
                dynamic InputB = null;
                int InputBDeci = 0;

                if (Numbers1parts != null)
                {
                    if (Counter >= Numbers1parts.Length)
                    {
                        InputA = Numbers1parts[Numbers1parts.GetUpperBound(0)];
                    }
                    else
                    {
                        InputA = Numbers1parts[Counter];
                    }

                    InputAString = Convert.ToString(InputA);
                }
                if (Numbers2parts != null)
                {
                    if (Counter >= Numbers2parts.Length)
                    {
                        InputB = Numbers2parts[Numbers2parts.GetUpperBound(0)]; ;
                    }
                    else
                    {
                        InputB = Numbers2parts[Counter];
                    }
                    int.TryParse(InputB, out InputBDeci);
                }

                if (parameters.Type == "Left")
                {
                    OutputValue = InputAString.Substring(0, InputBDeci);
                }
                else if (parameters.Type == "Right")
                {
                    OutputValue = InputAString.Substring(InputAString.Length - InputBDeci);
                }
                else if (parameters.Type == "Mid")
                {
                    OutputValue = "0";
                }
                else if (parameters.Type == "Find")
                {
                    int FindValue = InputAString.IndexOf(String2);

                    if( FindValue != -1)
                    {
                        OutputValue = Convert.ToString(FindValue);
                    }
                    else
                    {
                        OutputValue = "0";
                    }

                }
                else if (parameters.Type == "Len")
                {
                    OutputValue = Convert.ToString(InputAString.Length);
                }

                Output = Output + OutputValue + "~";

                Counter = Counter + 1;
            }

            Output = Output.Remove(Output.Length - 1);
            return Convert.ToString(Output);

        }

    }
}