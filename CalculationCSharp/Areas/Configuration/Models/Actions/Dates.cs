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

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            DateFunctions DatesFunctions = new DateFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            Dates parameters = (Dates)javaScriptSerializ­er.Deserialize(jparameters, typeof(Dates));
            List<string> D1parts = null;
            List<string> D2parts = null;
            string[] Date1parts = null;
            string[] Date2parts = null;

            D1parts = ArrayBuilder.InputArrayBuilder(parameters.Date1, jCategory, GroupID, ItemID);
            D2parts = ArrayBuilder.InputArrayBuilder(parameters.Date2, jCategory, GroupID, ItemID);

            if(D1parts != null)
            {
                Date1parts = D1parts.ToArray();
            }
            if(D2parts != null)
            {
                Date2parts = D2parts.ToArray();
            }

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

                }

                dynamic InputC = Config.VariableReplace(jCategory, parameters.Period, GroupID, ItemID);
                DateTime Date1;
                DateTime Date2;
                Decimal Period;


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
                string date = DatesFunctions.DateAdjustment(parameters.Type, Convert.ToString(Date1), Convert.ToString(Date2), parameters.PeriodType, Period, parameters.Adjustment, parameters.Day, parameters.Month);
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