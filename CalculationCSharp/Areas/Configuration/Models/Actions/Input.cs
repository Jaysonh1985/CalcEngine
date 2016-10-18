// Copyright (c) 2016 Project AIM
namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Input
    {
        /// <summary>Error handles the inputs that are used so that if the calculation has incorrect inputs the calculation can handle running and producing the relevant errors.
        /// <para>Type = Data type of the input</para>
        /// <para>Output = Value of the Output variable</para>
        /// </summary>
        public string Output (string Type, string Output)
        {
            if (Type == "Date")
            {
                if (Output != "" && Output != "0")
                {
                    return Output;
                }
                else
                {
                    return  "";
                }
            }
            else if (Type == "Decimal")
            {
                if (Output == "" || Output == null)
                {
                    return  "0";
                }
                else
                {
                    return Output;
                }
            }
            else
            {
                return Output;
            }         
        }
    }
}