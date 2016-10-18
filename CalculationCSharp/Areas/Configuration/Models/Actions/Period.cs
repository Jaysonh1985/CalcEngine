// Copyright (c) 2016 Project AIM
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

        /// <summary>Outputs where Period function is used, includes the array builder.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            DateFunctions DateFunctions = new DateFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            Period Dates = new Period();
            Period parameters = (Period)javaScriptSerializ­er.Deserialize(jparameters, typeof(Period));
            string[] Date1parts = null;
            string[] Date2parts = null;
            //Returns array
            Date1parts = ArrayBuilder.InputArrayBuilder(parameters.Date1, jCategory, GroupID, ItemID);
            Date2parts = ArrayBuilder.InputArrayBuilder(parameters.Date2, jCategory, GroupID, ItemID);
            string OutputValue = null;
            string Output = null;
            int Counter = 0;
            //Gets Max Length of array so loops through all values    
            int MaxLength = ArrayBuilder.GetMaxLength(Date1parts, Date2parts);
            //Loop through the array to calculate each value in array
            for (int i = 0; i < MaxLength; i++)
            {
                dynamic InputA = null;
                dynamic InputB = null;
                InputA = ArrayBuilder.GetArrayPart(Date1parts, Counter);
                InputB = ArrayBuilder.GetArrayPart(Date2parts, Counter);
                Counter = Counter + 1;
                //Checks if Inputs are OK to proceed in using the calculation
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
                        //Calcuates the relevant period
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
                        Output = Output + OutputValue + "~";
                    }
                    else
                    {
                        OutputValue = Convert.ToString(0);
                        Output = Output + OutputValue + "~";
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