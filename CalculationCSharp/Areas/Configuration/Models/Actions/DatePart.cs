// Copyright (c) 2016 Project AIM
using CalculationCSharp.Models.ArrayFunctions;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class DatePart
    {
        public string Part { get; set; }
        public dynamic Date1 { get; set; }
        /// <summary>Outputs where Date Part function is used includes the array builder.
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
            DatePart parameters = (DatePart)javaScriptSerializ­er.Deserialize(jparameters, typeof(DatePart));
            string[] Date1parts = null;
            //Returns Array
            Date1parts = ArrayBuilder.InputArrayBuilder(parameters.Date1, jCategory, GroupID, ItemID);
            string Output = null;
            //Loop through the array to calculate each value in array
            foreach (string part in Date1parts)
            {
                dynamic InputA = Config.VariableReplace(jCategory, part, GroupID, ItemID);
                DateTime Date1;
                DateTime.TryParse(InputA, out Date1);
                int DatePart = DatesFunctions.GetDatePart(parameters.Part, Date1);
                Output = Output + Convert.ToString(DatePart) + "~";
            }
            Output = Output.Remove(Output.Length - 1);
            return Convert.ToString(Output);
        }

    }
}