﻿using System;
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
                    if (Counter >= Date1parts.Length)
                    {
                        InputA = Config.VariableReplace(jCategory, Date1parts[Date1parts.GetUpperBound(0)], GroupID, ItemID);
                    }
                    else
                    {
                        InputA = Config.VariableReplace(jCategory, Date1parts[Counter], GroupID, ItemID);
                    }

                }
                if (Date2parts!= null)
                {
                    if(Counter >= Date2parts.Length)
                    {
                        InputB = Config.VariableReplace(jCategory, Date2parts[Date2parts.GetUpperBound(0)], GroupID, ItemID);
                    }
                    else
                    {
                        InputB = Config.VariableReplace(jCategory, Date2parts[Counter], GroupID, ItemID);
                    }
  
                }

                dynamic InputC = Config.VariableReplace(jCategory, parameters.Period, GroupID, ItemID);
                DateTime Date1;
                DateTime Date2;
                Decimal Period;

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
                    if(InputAparts != null)
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

                    if(InputBparts!= null)
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
                    

                    Decimal.TryParse(InputC, out Period);
                    DateTime date = DatesFunctions.DateAdjustment(parameters.Type, Convert.ToString(Date1), Convert.ToString(Date2), parameters.PeriodType, Period, parameters.Adjustment, parameters.Day, parameters.Month);
                    DateTime datestring = date.Date;
                    var shortdatestring = datestring.ToShortDateString();
                    Output = Output + Convert.ToString(shortdatestring) + ",";
                    InputCounter = InputCounter + 1;
                }
                Counter = Counter + 1;
            }       

            Output = Output.Remove(Output.Length - 1);
            return Convert.ToString(Output);
        }
    }
}