using System;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Period
    {
        public string DateAdjustmentType { get; set; }
        public dynamic Date1 { get; set; }
        public dynamic Date2 { get; set; }
        public bool Inclusive { get; set; }
        public double DaysinYear { get; set; }

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            DateFunctions DateFunctions = new DateFunctions();
            Period Dates = new Period();
            Period parameters = (Period)javaScriptSerializ­er.Deserialize(jparameters, typeof(Period));
            dynamic InputA = Config.VariableReplace(jCategory, parameters.Date1, GroupID, ItemID);
            dynamic InputB = Config.VariableReplace(jCategory, parameters.Date2, GroupID, ItemID);

            if (InputA != "" && InputB != "" && InputA != "01/01/0001" && InputB != "01/01/0001")
            {
                System.DateTime Date1 = DateTime.Parse(InputA);
                System.DateTime Date2 = DateTime.Parse(InputB);
                String DateAdjustmentType = parameters.DateAdjustmentType;
                Boolean Inclusive = parameters.Inclusive;
                Double DaysinYear = parameters.DaysinYear;

                if (Date1 <= Date2)
                {
                    if (DateAdjustmentType == "YearsDays")
                    {
                        return Convert.ToString(DateFunctions.YearsDaysBetween(Date1, Date2, Inclusive, DaysinYear));
                    }
                    else if (DateAdjustmentType == "YearsMonths")
                    {
                        return Convert.ToString(DateFunctions.YearsMonthsBetween(Date1, Date2, Inclusive, DaysinYear));
                    }

                    else if (DateAdjustmentType == "Years")
                    {
                        return Convert.ToString(DateFunctions.YearsBetween(Date1, Date2, Inclusive, DaysinYear));
                    }

                    else if (DateAdjustmentType == "Months")
                    {
                        return Convert.ToString(DateFunctions.GetMonthsBetween(Date1, Date2, false));
                    }

                    else if (DateAdjustmentType == "Days")
                    {
                        return Convert.ToString(DateFunctions.DaysBetween(Date1, Date2, Inclusive, DaysinYear));
                    }
                    else
                    {
                        return "0";
                    }

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