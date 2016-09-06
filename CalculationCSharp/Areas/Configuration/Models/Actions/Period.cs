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

            int Date1Length = 0;
            if (Date1parts != null)
            {
                Date1Length = Date1parts.Length;
            }

            int Date2Length = 0;
            if (Date2parts != null)
            {
                Date2Length = Date2parts.Length;
            }

            int MaxLength = Math.Max(Date1Length, Date2Length);

            for (int i = 0; i < MaxLength; i++)
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

                string[] InputAparts = null;
                string[] InputBparts = null;
                if (!string.IsNullOrEmpty(InputA))
                {
                    InputAparts = InputA.Split(',');
                }
                if (!string.IsNullOrEmpty(InputB))
                {
                    InputBparts = InputB.Split(',');
                }

                int InputALength = 0;
                if (InputAparts != null)
                {
                    InputALength = InputAparts.Length;
                }

                int InputBLength = 0;
                if (InputBparts != null)
                {
                    InputBLength = InputBparts.Length;
                }

                int InputsMaxLength = Math.Max(InputALength, InputBLength);
                int InputCounter = 0;
                for (int c = 0; c < InputsMaxLength; c++)
                {
    

                    if (InputA != "" && InputB != "" && InputA != "01/01/0001" && InputB != "01/01/0001")
                    {
                        DateTime Date1;
                        DateTime Date2;
                        if (InputAparts != null)
                        {
                            if (InputCounter >= InputAparts.Length)
                            {
                                DateTime.TryParse(InputAparts[InputAparts.GetUpperBound(0)], out Date1);
                            }
                            else
                            {
                                DateTime.TryParse(InputAparts[InputCounter], out Date1);
                            }
                        }
                        else
                        {
                            Date1 = Convert.ToDateTime("01/01/0001");
                        }

                        if (InputBparts != null)
                        {
                            if (InputCounter >= InputBparts.Length)
                            {
                                DateTime.TryParse(InputBparts[InputBparts.GetUpperBound(0)], out Date2);
                            }
                            else
                            {
                                DateTime.TryParse(InputBparts[InputCounter], out Date2);
                            }
                        }
                        else
                        {
                            Date2 = Convert.ToDateTime("01/01/0001");
                        }

                        InputCounter = InputCounter + 1;

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
            }
            if (Output != null)
            {
                Output = Output.Remove(Output.Length - 1);
            }
            return Output;
        }
    }
}