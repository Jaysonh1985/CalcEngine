using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

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

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            DateFunctions DatesFunctions = new DateFunctions();
            Dates parameters = (Dates)javaScriptSerializ­er.Deserialize(jparameters, typeof(Dates));
            dynamic InputA = Config.VariableReplace(jCategory, parameters.Date1, GroupID, ItemID);
            dynamic InputB = Config.VariableReplace(jCategory, parameters.Date2, GroupID, ItemID);
            dynamic InputC = Config.VariableReplace(jCategory, parameters.Period, GroupID, ItemID);
            DateTime Date1;
            DateTime Date2;
            Decimal Period;
            DateTime.TryParse(InputA, out Date1);
            DateTime.TryParse(InputB, out Date2);
            Decimal.TryParse(InputC, out Period);
            DateTime date = DatesFunctions.DateAdjustment(parameters.Type, Convert.ToString(Date1), Convert.ToString(Date2), parameters.PeriodType, Period, parameters.Adjustment, parameters.Day, parameters.Month);
            DateTime datestring = date.Date;
            var shortdatestring = datestring.ToShortDateString();
            return Convert.ToString(shortdatestring);
        }
    }
}