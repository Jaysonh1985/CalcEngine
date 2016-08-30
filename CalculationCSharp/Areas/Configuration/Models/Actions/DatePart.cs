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
            DatePart parameters = (DatePart)javaScriptSerializ­er.Deserialize(jparameters, typeof(DatePart));
            dynamic InputA = Config.VariableReplace(jCategory, parameters.Date1, GroupID, ItemID);
            DateTime Date1;
            DateTime.TryParse(InputA, out Date1);
            int DatePart = DatesFunctions.GetDatePart(parameters.Part, Date1);
            return Convert.ToString(DatePart);
        }

    }
}