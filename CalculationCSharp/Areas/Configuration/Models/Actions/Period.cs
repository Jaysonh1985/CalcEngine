using System;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using CalculationCSharp.Models.ArrayFunctions;

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
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            Period Dates = new Period();
            Period parameters = (Period)javaScriptSerializ­er.Deserialize(jparameters, typeof(Period));

            List<string> D1parts = null;
            List<string> D2parts = null;
            string[] Date1parts = null;
            string[] Date2parts = null;

            D1parts = ArrayBuilder.InputArrayBuilder(parameters.Date1, jCategory, GroupID, ItemID);
            D2parts = ArrayBuilder.InputArrayBuilder(parameters.Date2, jCategory, GroupID, ItemID);

            if (D1parts != null)
            {
                Date1parts = D1parts.ToArray();
            }
            if (D2parts != null)
            {
                Date2parts = D2parts.ToArray();
            }

            string OutputValue = null;
            string Output = null;
            int Counter = 0;

            int MaxLength = ArrayBuilder.GetMaxLength(Date1parts, Date2parts);

            for (int i = 0; i < MaxLength; i++)
            {
                dynamic InputA = null;
                dynamic InputB = null;

                if (Date1parts != null)
                {
                    if (Counter >= Date1parts.Length)
                    {
                        InputA = Date1parts[Date1parts.GetUpperBound(0)];
                    }
                    else
                    {
                        InputA = Date1parts[Counter];
                    }

                }
                if (Date2parts != null)
                {
                    if (Counter >= Date2parts.Length)
                    {
                        InputB = Date2parts[Date2parts.GetUpperBound(0)]; ;
                    }
                    else
                    {
                        InputB = Date2parts[Counter];
                    }

                    Counter = Counter + 1;
                }

                    if (InputA != "" && InputB != "" && InputA != "01/01/0001" && InputB != "01/01/0001")
                    {
                        DateTime Date1;
                        DateTime Date2;
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
            
            if (Output != null)
            {
                Output = Output.Remove(Output.Length - 1);
            }
            return Output;
        }
    }
}