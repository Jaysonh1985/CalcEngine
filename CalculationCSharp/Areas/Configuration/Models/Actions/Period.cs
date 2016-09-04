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

            string[] Date1parts = null;
            string[] Date2parts = null;
            if (!string.IsNullOrEmpty(parameters.Date1))
            {
                Date1parts = parameters.Date1.Split(',');
            }
            if (!string.IsNullOrEmpty(parameters.Date2))
            {
                Date2parts = parameters.Date2.Split(',');
            }

            string OutputValue = null;
            string Output = null;
            int Counter = 0;

            foreach (string part in Date1parts)
            {

                dynamic InputA = null;
                dynamic InputB = null;

                if (Date1parts != null)
                {
                    InputA = Config.VariableReplace(jCategory, Date1parts[Counter], GroupID, ItemID);
                }
                if (Date2parts != null)
                {
                    if (Counter >= Date2parts.Length)
                    {
                        InputB = Config.VariableReplace(jCategory, Date2parts[Date2parts.GetUpperBound(0)], GroupID, ItemID);
                    }
                    else
                    {
                        InputB = Config.VariableReplace(jCategory, Date2parts[Counter], GroupID, ItemID);
                    }


                }
                Counter = Counter + 1;

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
                            OutputValue = Convert.ToString(DateFunctions.YearsDaysBetween(Date1, Date2, Inclusive, DaysinYear));
                        }
                        else if (DateAdjustmentType == "YearsMonths")
                        {
                            OutputValue = Convert.ToString(DateFunctions.YearsMonthsBetween(Date1, Date2, Inclusive, DaysinYear));
                        }

                        else if (DateAdjustmentType == "Years")
                        {
                            OutputValue = Convert.ToString(DateFunctions.YearsBetween(Date1, Date2, Inclusive, DaysinYear));
                        }

                        else if (DateAdjustmentType == "Months")
                        {
                            OutputValue = Convert.ToString(DateFunctions.GetMonthsBetween(Date1, Date2, false));
                        }

                        else if (DateAdjustmentType == "Days")
                        {
                            OutputValue = Convert.ToString(DateFunctions.DaysBetween(Date1, Date2, Inclusive, DaysinYear));
                        }
                        else
                        {
                            OutputValue = "0";
                        }


                    }
                    else
                    {
                        OutputValue = Convert.ToString(0);
                    }
                    Output = Output + OutputValue + ",";
                }
                else
                {
                    OutputValue = Convert.ToString(0);
                    Output = Output + OutputValue + ",";
                }
            }
            Output = Output.Remove(Output.Length - 1);

            return Output;
        }
    }
}