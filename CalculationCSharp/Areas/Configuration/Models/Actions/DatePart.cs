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

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            DateFunctions DatesFunctions = new DateFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            DatePart parameters = (DatePart)javaScriptSerializ­er.Deserialize(jparameters, typeof(DatePart));
            List<string> D1parts = null;
            string[] Date1parts = null;
            D1parts = ArrayBuilder.InputArrayBuilder(parameters.Date1, jCategory, GroupID, ItemID);

            if (D1parts != null)
            {
                Date1parts = D1parts.ToArray();
            }

            string Output = null;
            foreach(string part in Date1parts)
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