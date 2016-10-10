// Copyright (c) 2016 Project AIM
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

public class DateFunctions
{

	public double YearsMonthsBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive, double daysperyear)
	{
        int months = GetMonthsBetween(StartDate, EndDate, inclusive);

        double output = months / 12;

        double YearsMonths = ((months - (output *12))/100) + output;
        
        return YearsMonths;
	}

	 public double YearsDaysBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive, double daysperyear)
	{
        System.TimeSpan d;
        double days;
        d = EndDate.Subtract(StartDate);
        days = d.Days;
        if(inclusive == true)
        {
            days = days + 1;
        }
        double Years = Math.Floor(days / daysperyear);
		days = (days / daysperyear) - Years;
        days = Math.Floor(days * daysperyear);
        days = Math.Round((days / 1000),3,MidpointRounding.AwayFromZero);
        days = Years + days;
        return days;
	}

    public double YearsBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive, double daysperyear)
    {
        System.TimeSpan d;
        double days;
        d = EndDate.Subtract(StartDate);
        days = d.Days;
        double Years = Math.Floor(days / daysperyear);
        return Years;
    }

    public int GetMonthsBetween(System.DateTime StartDate, System.DateTime EndDate, bool roundup)
	{
        if(roundup == true)
        {
            EndDate = EndDate.AddDays(1);
        }
        //if (StartDate > EndDate) return GetMonthsBetween(EndDate, StartDate, true);
        var monthDiff = Math.Abs((EndDate.Year * 12 + (EndDate.Month - 1)) - (StartDate.Year * 12 + (StartDate.Month - 1)));
        if (StartDate.AddMonths(monthDiff) > EndDate || EndDate.Day < StartDate.Day)
        {
            return monthDiff - 1;
        }
        else
        {
            return monthDiff;
        }
  //      int i = 1;
		//if (EndDate < StartDate) {
		//	i = i - 1;

		//	StartDate = EndDate;
		//	EndDate = StartDate;
		//}

  //      int Years = EndDate.Year - StartDate.Year;
  //      int Months = (EndDate.Month - StartDate.Month);

		//if (EndDate.Day < StartDate.Day) {
		//	Months = Months - 1;
		//}

		//if ((roundup == true & EndDate.Day != StartDate.Day)) {
		//	Months = Months + 1;
		//}

		//return (Years*12)+ Months;

	}

    public double DaysBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive, double daysperyear)
    {
        System.TimeSpan d;
        double days;
        d = EndDate.Subtract(StartDate);
        days = d.Days;
        if (inclusive == true)
        {
            days = days + 1;
        }
        return days;
    }

    public string DateAdjustment(string Type, String DateA, String DateB, string PeriodType, decimal Period, string Adjustment, string Day, string Month)
    {
        DateTime Date1;
        DateTime Date2;
        DateTime.TryParse(DateA, out Date1);
        DateTime.TryParse(DateB, out Date2);
        if (Type == "Add" || Type == "Subtract")
        {
                decimal Yrs = Math.Floor(Period) ;
                decimal Frac = (Period - Yrs);
                decimal Number;
                string outPut = "0";
                if (Period.ToString().Split('.').Length == 2)
                {
                    outPut = Period.ToString().Split('.')[1].Substring(0, Period.ToString().Split('.')[1].Length);
                }
                if (decimal.TryParse(outPut, out Number))
                {
                    Frac = Number;
                }
                else
                {
                    Frac = 0;
                };
            if (PeriodType == "YearsDays")
            {
                Yrs = Yrs * Convert.ToDecimal(365.25);
                Double Total = Convert.ToDouble(Yrs + Frac);
                if (Type == "Subtract")
                {
                    Total = Total * -1;
                }
                DateTime AdjDate = Date1.AddDays(Total);
                return AdjDate.ToShortDateString();
            }

            if (PeriodType == "YearsMonths")
            {
                Yrs = Yrs * Convert.ToDecimal(12);
                int Total = Convert.ToInt32(Yrs + Frac);
                if (Type == "Subtract")
                {
                    Total = Total * -1;
                }
                DateTime AdjDate = Date1.AddMonths(Total);
                return AdjDate.ToShortDateString();
            }

            if (PeriodType == "Years")
            {
                Yrs = Yrs * Convert.ToDecimal(12);
                int Total = Convert.ToInt32(Yrs + Frac);
                if (Type == "Subtract")
                {
                    Total = Total * -1;
                }
                DateTime AdjDate = Date1.AddMonths(Total);
                return AdjDate.ToShortDateString();
            }

            if (PeriodType == "Months")
            {
                int Total = Convert.ToInt32(Yrs);
                if (Type == "Subtract")
                {
                    Total = Total * -1;
                }
                DateTime AdjDate = Date1.AddMonths(Total);
                return AdjDate.ToShortDateString();
            }
        }
        else if (Type == "Adjust")
        {
            string date = Day;
            string[] dateTokens = date.Split('/');
            string strDay = Day;
            string strMonth = Month;
            int intDay = Convert.ToInt32(strDay);
            int intMonth = Convert.ToInt32(strMonth);
            DateTime Value = new DateTime(Date1.Year, intMonth, intDay);
            if (Adjustment == "Less")
            {
                if (Value < Date1)
                {
                    return Value.ToShortDateString();
                }
                else if (Value == Date1)
                {
                    Value = new DateTime(Date1.Year - 1, intMonth, intDay);
                    return Value.ToShortDateString();
                }
                else
                {
                    Value = new DateTime(Date1.Year - 1, intMonth, intDay);
                    return Value.ToShortDateString();
                }
            }
            else if(Adjustment == "LessEqual")
            {
                if (Value < Date1)
                {
                    return Value.ToShortDateString();
                }
                else if (Value == Date1)
                {
                    return Value.ToShortDateString();
                }
                else
                {
                    Value = new DateTime(Date1.Year - 1, intMonth, intDay);
                    return Value.ToShortDateString();
                }
            }
            else if(Adjustment == "Greater")
            {
                if (Value > Date1)
                {
                    return Value.ToShortDateString();
                }
                else if (Value == Date1)
                {
                    Value = new DateTime(Date1.Year + 1, intMonth, intDay);
                    return Value.ToShortDateString();
                }
                else
                {
                    Value = new DateTime(Date1.Year + 1, intMonth, intDay);
                    return Value.ToShortDateString();
                }
            }
            else if (Adjustment == "GreaterEqual")
            {
                if (Value >= Date1)
                {
                    return Value.ToShortDateString();
                }
                else if (Value == Date1)
                {
                    return Value.ToShortDateString();
                }
                else
                {
                    Value = new DateTime(Date1.Year + 1, intMonth, intDay);
                    return Value.ToShortDateString();
                }
            }
            else if (Adjustment == "Equal")
            {
                   Value = new DateTime(Date1.Year, intMonth, intDay);
                   return Value.ToShortDateString();
            }
        }

        else if (Type == "Earlier")
        {
            int result = DateTime.Compare(Date1, Date2);
            if(result < 0)
            {
                return Date1.ToShortDateString();
            }
            else if(result > 0)
            {
                return Date2.ToShortDateString();
            }
            else
            {
                return Date1.ToShortDateString();
            }
        }
        else if (Type == "Later")
        {
            int result = DateTime.Compare(Date1, Date2);
            if (result < 0)
            {
                return Date2.ToShortDateString();
            }
            else if (result > 0)
            {
                return Date1.ToShortDateString();
            }
            else
            {
                return Date1.ToShortDateString();
            }
        }
        else if(Type == "DatesBetween")
        {
            if(Date1 <= Date2)
            {
                var DatesList = new List<String>();
                string strDay = Day;
                string strMonth = Month;
                int intDay = Convert.ToInt32(strDay);
                int intMonth = Convert.ToInt32(strMonth);
                DateTime dateIncrement = Date1;
                DateTime FirstValue = new DateTime(Date1.Year, intMonth, intDay);
                if (FirstValue > Date1)
                {
                    dateIncrement = new DateTime(Date1.Year, intMonth, intDay);
                }
                else
                {
                    dateIncrement = new DateTime(Date1.Year + 1, intMonth, intDay);
                }
                if(dateIncrement < Date2)
                {
                    /* do loop execution */
                    do
                    {
                        DatesList.Add(dateIncrement.ToShortDateString());
                        dateIncrement = dateIncrement.AddYears(1);
                    }
                    while (dateIncrement <= Date2);
                    return string.Join("~", DatesList.ToArray());
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        return Convert.ToDateTime("01/01/1900").ToShortDateString(); 
    }

    public int GetDatePart(string Part, System.DateTime Date)
    {
        if(Part == "Year")
        {
            return Date.Year;
        }
        else if(Part == "Month")
        {
            return Date.Month;
        }
        else if(Part == "Day")
        {
            return Date.Day;
        }
        else
        {
            return 0;
        }

    }
}