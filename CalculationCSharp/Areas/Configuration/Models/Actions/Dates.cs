// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using CalculationCSharp.Models.ArrayFunctions;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Dates
    {
        public string Type { get; set; }
        public dynamic Date1 { get; set; }
        public dynamic Date2 { get; set; }
        public string PeriodType { get; set; }
        public dynamic Period { get; set; }
        public string Adjustment { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        /// <summary>Outputs Date Adjustment function is used, includes the array builder.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
            DateFunctions DatesFunctions = new DateFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            Dates parameters = (Dates)javaScriptSerializ­er.Deserialize(jparameters, typeof(Dates));
            if(parameters.Type == "Today")
            {
                return DateTime.Now.ToShortDateString();
            }
            string[] Date1parts = null;
            string[] Date2parts = null;
            //Returns array
            Date1parts = ArrayBuilder.InputArrayBuilder(parameters.Date1, jCategory, GroupID, ItemID);
            Date2parts = ArrayBuilder.InputArrayBuilder(parameters.Date2, jCategory, GroupID, ItemID);
            string Output = null;
            int Counter = 0;
            //Gets Max Length of array so loops through all values
            int MaxLength = ArrayBuilder.GetMaxLength(Date1parts, Date2parts);
            //Loop through the array to calculate each value in array
            for (int i = 0; i < MaxLength; i++)
            {
                dynamic InputA = null;
                dynamic InputB = null;
                //Gets the current array to use in the loop
                InputA = ArrayBuilder.GetArrayPart(Date1parts, Counter);
                InputB = ArrayBuilder.GetArrayPart(Date2parts, Counter);
                dynamic InputC = Config.VariableReplace(jCategory, parameters.Period, GroupID, ItemID);
                dynamic InputD = Config.VariableReplace(jCategory, parameters.Day, GroupID, ItemID);
                dynamic InputE = Config.VariableReplace(jCategory, parameters.Month, GroupID, ItemID);
                DateTime Date1;
                DateTime Date2;
                Decimal Period;
                //Data output checker
                if (InputA != null)
                {
                    DateTime.TryParse(InputA, out Date1);
                }
                else
                {
                    Date1 = Convert.ToDateTime("01/01/0001");
                }

                if (InputB != null)
                {
                    DateTime.TryParse(InputB, out Date2);
                }
                else
                {
                    Date2 = Convert.ToDateTime("01/01/0001");
                }
                Decimal.TryParse(InputC, out Period);
                string date = DatesFunctions.DateAdjustment(parameters.Type, Convert.ToString(Date1), Convert.ToString(Date2), parameters.PeriodType, Period, parameters.Adjustment, InputD, InputE);
                Output = Output + date + "~";
                Counter = Counter + 1;
            }
            if(Output != null)
            {
                Output = Output.Remove(Output.Length - 1);
            }           
            return Convert.ToString(Output);
        }
    }
}