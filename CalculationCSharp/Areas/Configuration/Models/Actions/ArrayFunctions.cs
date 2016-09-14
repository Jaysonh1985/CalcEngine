using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ArrayFunctions
    {
        public string LookupType { get; set; }
        public dynamic LookupValue { get; set; }
        public string Function { get; set; }

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output (string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            MathematicalFunctions MathFunctions = new MathematicalFunctions();
            ArrayFunctions parameters = (ArrayFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(ArrayFunctions));
            dynamic InputA = Config.VariableReplace(jCategory, parameters.LookupValue, GroupID, ItemID);
            string OutputValue = null;

            if(parameters.LookupType == "Decimal")
            {
                if (parameters.Function == "Sum")
                {
                    string[] Deci1parts = null;
                    Deci1parts = InputA.Split('~');
                    decimal Sum = 0;

                    foreach (string output in Deci1parts)
                    {
                        decimal InputADeci = 0;
                        decimal.TryParse(output, out InputADeci);
                        Sum = decimal.Add(Sum, InputADeci);
                    }
                    OutputValue = Convert.ToString(Sum);
                }
                else if (parameters.Function == "Product")
                {
                    string[] Deci1parts = null;
                    Deci1parts = InputA.Split('~');
                    decimal Sum = 0;
                    int counter = 0;
                    foreach (string output in Deci1parts)
                    {
                        decimal InputADeci = 0;
                        decimal.TryParse(output, out InputADeci);
                        if (counter == 0)
                        {
                            Sum = InputADeci;
                        }
                        else
                        {
                            Sum = decimal.Multiply(Sum, InputADeci);
                        }

                        counter = counter + 1;
                    }
                    OutputValue = Convert.ToString(Sum);
                }
                else if (parameters.Function == "FirstNumber")
                {
                    string[] Deci1parts = null;
                    Deci1parts = InputA.Split('~');
                    OutputValue = Convert.ToString(Deci1parts[0]);

                }
                else if (parameters.Function == "LastNumber")
                {
                    string[] Deci1parts = null;
                    Deci1parts = InputA.Split('~');
                    OutputValue = Convert.ToString(Deci1parts[Deci1parts.GetUpperBound(0)]);
                }
                else if (parameters.Function == "Count")
                {
                    string[] Deci1parts = null;
                    Deci1parts = InputA.Split('~');
                    OutputValue = Convert.ToString(Deci1parts.Length);
                }
                else
                {
                    OutputValue = Convert.ToString(0);
                }

            }
            else if(parameters.LookupType == "Date")
            {

                if (parameters.Function == "FirstDate")
                {
                    string[] Date1parts = null;
                    Date1parts = InputA.Split('~');
                    OutputValue = Convert.ToString(Date1parts[0]);
                }
                else if(parameters.Function == "LastDate")
                {
                    string[] Date1parts = null;
                    Date1parts = InputA.Split('~');           
                    OutputValue = Convert.ToString(Date1parts[Date1parts.GetUpperBound(0)]);
                }
                else if (parameters.Function == "Count")
                {
                    string[] Deci1parts = null;
                    if(InputA != "")
                    {
                        Deci1parts = InputA.Split('~');
                        OutputValue = Convert.ToString(Deci1parts.Length);
                    }
                    else
                    {
                        OutputValue = Convert.ToString(0);
                    }
                }
                else
                {
                    OutputValue = Convert.ToString(0);
                }

            }
            else
            {
                OutputValue = Convert.ToString(0);
            }


            return Convert.ToString(OutputValue);
        }

    }
}