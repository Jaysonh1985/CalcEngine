// Copyright (c) 2016 Project AIM
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
        public dynamic String3 { get; set; }
        /// <summary>Outputs where String Functions function is used, includes the array builder.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public string Output (string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            MathematicalFunctions MathFunctions = new MathematicalFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            StringFunctions parameters = (StringFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(StringFunctions));
            string[] Numbers1parts = null;
            string[] Numbers2parts = null;
            string[] Numbers3parts = null;
            //Returns array
            Numbers1parts = ArrayBuilder.InputArrayBuilder(parameters.String1, jCategory, GroupID, ItemID);
            Numbers2parts = ArrayBuilder.InputArrayBuilder(parameters.Number1, jCategory, GroupID, ItemID);
            Numbers3parts = ArrayBuilder.InputArrayBuilder(parameters.String3, jCategory, 0, 0);
            string Output = null;
            string OutputValue = null;
            //Gets Max Length of array so loops through all values  
            int MaxLength = ArrayBuilder.GetMaxLength(Numbers1parts, Numbers2parts);
            //If Set only then
            if(Numbers3parts != null)
            {
                MaxLength = 1;
            }
            int Counter = 0;
            //Loop through the array to calculate each value in array
            for (int i = 0; i < MaxLength; i++)
            {
                dynamic InputA = null;
                dynamic InputB = null;
                dynamic InputC = null;
                //Gets the current array to use in the loop
                InputA = ArrayBuilder.GetArrayPart(Numbers1parts, Counter);
                InputB = ArrayBuilder.GetArrayPart(Numbers2parts, Counter);
                InputC = ArrayBuilder.GetArrayPart(Numbers3parts, Counter);

                string InputAString = null;               
                int InputBDeci = 0;
                string InputCString = null;
                
                InputAString = Convert.ToString(InputA);
                int.TryParse(InputB, out InputBDeci);
                InputCString = Convert.ToString(InputC);
                //Calculates the value required
                if (parameters.Type == "Left")
                {
                    OutputValue = InputAString.Substring(0, InputBDeci);
                }
                else if (parameters.Type == "Right")
                {
                    OutputValue = InputAString.Substring(InputAString.Length - InputBDeci);
                }
                else if (parameters.Type == "Set")
                {
                    OutputValue = InputCString;
                }
                else if (parameters.Type == "Find")
                {
                    int FindValue = InputAString.IndexOf(parameters.String2);

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