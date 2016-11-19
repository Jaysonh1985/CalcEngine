// Copyright (c) 2016 Project AIM
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
        public string PeriodType { get; set; }
        /// <summary>Outputs where Array Functions function is used, includes the array builder.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public string Output (string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            MathematicalFunctions MathFunctions = new MathematicalFunctions();
            ArrayFunctions parameters = (ArrayFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(ArrayFunctions));
            dynamic InputA = Config.VariableReplace(jCategory, parameters.LookupValue, GroupID, ItemID);
            string OutputValue = null;

            if (parameters.LookupType == "Decimal")
            {
                OutputValue = DecimalCalculation(parameters, InputA);
            }
            else if(parameters.LookupType == "Date")
            {
                OutputValue = DateCalculation(parameters, InputA);
            }
            else
            {
                OutputValue = Convert.ToString(0);
            }
            return Convert.ToString(OutputValue);
        }
        /// <summary>Runs the Array Decimal calculation.
        /// <para>parameters = JSON congifurations relating to this function</para>
        /// <para>InputA = value to be passed into the calculation</para>
        /// </summary>
        public string DecimalCalculation(ArrayFunctions parameters, dynamic InputA)
        {
            MathematicalFunctions MathFunctions = new MathematicalFunctions();
            //Sums up all values in the array
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
                return Convert.ToString(Sum);
            }
            //Multiplies all values in the array
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
                return Convert.ToString(Sum);
            }
            //Gets the first number in the array
            else if (parameters.Function == "FirstNumber")
            {
                string[] Deci1parts = null;
                Deci1parts = InputA.Split('~');
                return Convert.ToString(Deci1parts[0]);
            }
            //Gets the last number in the array
            else if (parameters.Function == "LastNumber")
            {
                string[] Deci1parts = null;
                Deci1parts = InputA.Split('~');
                return Convert.ToString(Deci1parts[Deci1parts.GetUpperBound(0)]);
            }
            //Calculates Total Period in the array 
            else if (parameters.Function == "TotalPeriod")
            {
                string[] Deci1parts = null;
                Deci1parts = InputA.Split('~');
                decimal Sum = 0;
                int counter = 0;
                bool firstpass = false;
                foreach (string output in Deci1parts)
                {   
                    if(firstpass == false)
                    {
                        if(Deci1parts.Length > 1)
                        {
                            Sum = MathFunctions.AddPeriod(Convert.ToDecimal(output), Convert.ToDecimal(Deci1parts[counter + 1]), parameters.PeriodType);
                        }
                        else
                        {
                            Sum = Convert.ToDecimal(output);
                        }
                        
                        firstpass = true;
                    }      
                    else if (Deci1parts.Length > 2 && counter >= 2)
                    {
                        Sum = MathFunctions.AddPeriod(Sum, Convert.ToDecimal(output), parameters.PeriodType);
                    }        
                  
                    counter = counter + 1;
                }
               
                if(parameters.PeriodType == "YearsDays")
                {
                    return MathFunctions.RoundingDecimalPlaces("3", Sum);
                }
                else
                {
                    return MathFunctions.RoundingDecimalPlaces("2", Sum);
                }

               

            }
            //Counts the number or values in the array
            else if (parameters.Function == "Count")
            {
                string[] Deci1parts = null;
                Deci1parts = InputA.Split('~');
                return Convert.ToString(Deci1parts.Length);
            }
            else
            {
                return Convert.ToString(0);
            }
        }
        /// <summary>Runs the Array Date calculation.
        /// <para>parameters = JSON congifurations relating to this function</para>
        /// <para>InputA = value to be passed into the calculation</para>
        /// </summary>
        public string DateCalculation(ArrayFunctions parameters, dynamic InputA)
        {
            //Gets the First Date in the array
            if (parameters.Function == "FirstDate")
            {
                string[] Date1parts = null;
                Date1parts = InputA.Split('~');
                return Convert.ToString(Date1parts[0]);
            }
            //Gets the Last Date in the array
            else if (parameters.Function == "LastDate")
            {
                string[] Date1parts = null;
                Date1parts = InputA.Split('~');
                return Convert.ToString(Date1parts[Date1parts.GetUpperBound(0)]);
            }
            //Counts the number of instances
            else if (parameters.Function == "Count")
            {
                string[] Deci1parts = null;
                if (InputA != "")
                {
                    Deci1parts = InputA.Split('~');
                    return Convert.ToString(Deci1parts.Length);
                }
                else
                {
                   return Convert.ToString(0);
                }
            }
            else
            {
                return Convert.ToString(0);
            }
        }
    }
}