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
            string Output = null;
            Decimal OutputValue = 0;
            string[] Numbers1parts = null;
            string[] Numbers2parts = null;
            if (!string.IsNullOrEmpty(parameters.Number1))
            {
                Numbers1parts = parameters.Number1.Split(',');
            }
            if (!string.IsNullOrEmpty(parameters.Number2))
            {
                Numbers2parts = parameters.Number2.Split(',');
            }

            int Numbers1Length = 0;
            if (Numbers1parts != null)
            {
                Numbers1Length = Numbers1parts.Length;
            }

            int Numbers2Length = 0;
            if (Numbers2parts != null)
            {
                Numbers2Length = Numbers2parts.Length;
            }

            int MaxLength = Math.Max(Numbers1Length, Numbers2Length);

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

                string[] InputAparts = null;
                string[] InputBparts = null;
                if (!string.IsNullOrEmpty(InputA))
                {
                    InputAparts = InputA.Split(',');
                }
                if (!string.IsNullOrEmpty(InputB))
                {
                    InputBparts = InputB.Split(',');
                }

                int InputALength = 0;
                if (InputAparts != null)
                {
                    InputALength = InputAparts.Length;
                }

                int InputBLength = 0;
                if (InputBparts != null)
                {
                    InputBLength = InputBparts.Length;
                }

                int InputsMaxLength = Math.Max(InputALength, InputBLength);
                int InputCounter = 0;

                for (int c = 0; c < InputsMaxLength; c++)
                {
                    if (InputAparts != null)
                    {
                        if (InputCounter >= InputAparts.Length)
                        {
                            Decimal.TryParse(InputAparts[InputAparts.GetUpperBound(0)], out InputADeci);
                        }
                        else
                        {
                            Decimal.TryParse(InputAparts[InputCounter], out InputADeci);
                        }
                    }
                    else
                    {
                        InputADeci = 0;
                    }
                    if (InputBparts != null)
                    {
                        if (InputCounter >= InputBparts.Length)
                        {
                            Decimal.TryParse(InputBparts[InputBparts.GetUpperBound(0)], out InputBDeci);
                        }
                        else
                        {
                            Decimal.TryParse(InputBparts[InputCounter], out InputBDeci);
                        }
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
                    InputCounter = InputCounter + 1;
                }
                Counter = Counter + 1;  

            }
            Output = Output.Remove(Output.Length - 1);
            return Convert.ToString(Output);
        }

    }
}