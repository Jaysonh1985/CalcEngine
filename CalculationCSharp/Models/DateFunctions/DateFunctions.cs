// Copyright (c) 2016 Project AIM
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

public class DateFunctions
{
    /// <summary>Various Date Adjustment Functions.
    /// </summary>

    /// <summary>Years Months Between 2 dates this will output a decimal to 2dp with the dp representing months.
    /// <para>StartDate = Start Date for the calculation</para>
    /// <para>EndDate = End Date for the calculation</para>
    /// <para>inclusive = Adds 1 day on the end</para>
    /// <para>daysperyear = set days per year</para>
    /// </summary>
    public double YearsMonthsBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive, double daysperyear)
	{
        int months = GetMonthsBetween(StartDate, EndDate, inclusive);
        double output = months / 12;
        double YearsMonths = ((months - (output *12))/100) + output;     
        return YearsMonths;
	}
    /// <summary>Years Days Between 2 dates this will output a decimal to 3dp with the dp representing days.
    /// <para>StartDate = Start Date for the calculation</para>
    /// <para>EndDate = End Date for the calculation</para>
    /// <para>inclusive = Adds 1 day on the end</para>
    /// <para>daysperyear = set days per year</para>
    /// </summary>
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
    /// <summary>Years Between 2 dates this will output a decimal to 0dp
    /// <para>StartDate = Start Date for the calculation</para>
    /// <para>EndDate = End Date for the calculation</para>
    /// <para>inclusive = Adds 1 day on the end</para>
    /// <para>daysperyear = set days per year</para>
    /// </summary>
    public double YearsBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive, double daysperyear)
    {
        System.TimeSpan d;
        double days;
        d = EndDate.Subtract(StartDate);
        days = d.Days;
        double Years = Math.Floor(days / daysperyear);
        return Years;
    }
    /// <summary>Months Between 2 dates this will output a decimal to 0dp
    /// <para>StartDate = Start Date for the calculation</para>
    /// <para>EndDate = End Date for the calculation</para>
    /// <para>roundup = round up months from mid point up</para>
    /// </summary>
    public int GetMonthsBetween(System.DateTime StartDate, System.DateTime EndDate, bool roundup)
	{
        if(roundup == true)
        {
            EndDate = EndDate.AddDays(1);
        }
        var monthDiff = Math.Abs((EndDate.Year * 12 + (EndDate.Month - 1)) - (StartDate.Year * 12 + (StartDate.Month - 1)));
        if (StartDate.AddMonths(monthDiff) > EndDate || EndDate.Day < StartDate.Day)
        {
            return monthDiff - 1;
        }
        else
        {
            return monthDiff;
        }
	}
    /// <summary>Days Between 2 dates this will output a decimal to 0dp
    /// <para>StartDate = Start Date for the calculation</para>
    /// <para>EndDate = End Date for the calculation</para>
    /// <para>inclusive = Adds 1 day on the end</para>
    /// <para>daysperyear = set days per year</para>
    /// </summary>
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
    /// <summary>Controller to determine which Date Adjustment Calculation to run
    /// <para>Type = Date Adjustment Type</para>
    /// <para>DateA = First Date</para>
    /// <para>DateB = Second Date</para>
    /// <para>PeriodType = Period Type determines the type of adjustment to apply to the date years months/days</para>
    /// <para>Period = Period amount to apply to a date</para>
    /// <para>Adjustment = Adjustment direction to apply to adjustment function</para>
    /// <para>Day = Day to set adjusted date to</para>
    /// <para>Month = Month to set adjusted date to</para>
    /// </summary>
    public string DateAdjustment(string Type, String DateA, String DateB, string PeriodType, decimal Period, string Adjustment, string Day, string Month)
    {
        DateTime Date1;
        DateTime Date2;
        DateTime.TryParse(DateA, out Date1);
        DateTime.TryParse(DateB, out Date2);
        if (Type == "Add" || Type == "Subtract")
        {
            return DateAdjustmentAddSubtract(Period, PeriodType, Type, Date1);
        }
        else if (Type == "Adjust")
        {
            return DateAdjustmentAdjustment(Day, Month, Date1, Adjustment);
        }
        else if (Type == "Earlier")
        {
            return DateAdjustmentEarlier(Date1, Date2);
        }
        else if (Type == "Later")
        {
            return DateAdjustmentLater(Date1, Date2);
        }
        else if(Type == "DatesBetween")
        {
            return DateAdjustmentDatesBetween(Date1, Date2, Day, Month);
        }
        else
        {
            return Convert.ToDateTime("01/01/1900").ToShortDateString();
        }
    }
    /// <summary>Add period to a date
    /// <para>Type = Date Adjustment Type</para>
    /// <para>Date1 = Base date to add period to</para>
    /// <para>PeriodType = Period Type determines the type of adjustment to apply to the date years months/days</para>
    /// <para>Period = Period amount to apply to a date</para>
    /// </summary>
    public string DateAdjustmentAddSubtract(Decimal Period, string PeriodType, string Type, DateTime Date1)
    {
        decimal Yrs = Math.Floor(Period);
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
        }
        DateTime AdjDate;
        if (PeriodType == "YearsDays")
        {
            Yrs = Yrs * Convert.ToDecimal(365.25);
            Double Total = Convert.ToDouble(Yrs + Frac);
            if (Type == "Subtract")
            {
                Total = Total * -1;
            }
            AdjDate = Date1.AddDays(Total);
        }
        else if (PeriodType == "YearsMonths")
        {
            Yrs = Yrs * Convert.ToDecimal(12);
            int Total = Convert.ToInt32(Yrs + Frac);
            if (Type == "Subtract")
            {
                Total = Total * -1;
            }
            AdjDate = Date1.AddMonths(Total);
        }
        else if (PeriodType == "Years")
        {
            Yrs = Yrs * Convert.ToDecimal(12);
            int Total = Convert.ToInt32(Yrs + Frac);
            if (Type == "Subtract")
            {
                Total = Total * -1;
            }
            AdjDate = Date1.AddMonths(Total);
        }
        else if (PeriodType == "Months")
        {
            int Total = Convert.ToInt32(Yrs);
            if (Type == "Subtract")
            {
                Total = Total * -1;
            }
            AdjDate = Date1.AddMonths(Total);
        }
        else
        {
            AdjDate = Date1.AddMonths(0);
        }
        return AdjDate.ToShortDateString();
    }
    /// <summary>Adjust a date to a new date either in the year prior or after
    /// <para>Date1 = First Date</para>
    /// <para>Adjustment = Adjustment direction to apply to adjustment function</para>
    /// <para>Day = Day to set adjusted date to</para>
    /// <para>Month = Month to set adjusted date to</para>
    /// </summary>
    public string DateAdjustmentAdjustment(String Day, String Month, DateTime Date1, String Adjustment)
    {
        string date = Day;
        string[] dateTokens = date.Split('/');
        string strDay = Day;
        string strMonth = Month;
        int intDay = Convert.ToInt32(strDay);
        int intMonth = Convert.ToInt32(strMonth);
        DateTime Value = new DateTime(Date1.Year, intMonth, intDay);
        //Adjust to date before current date
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
        //Adjust to date before or on current date
        else if (Adjustment == "LessEqual")
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
        //Adjust to date after current date
        else if (Adjustment == "Greater")
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
        //Adjust to date on or after current date
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
        else
        {
            Value = new DateTime(Date1.Year, intMonth, intDay);
            return Value.ToShortDateString();
        }
    }
    /// <summary>Find Earlier of two dates
    /// <para>Date1 = First Date</para>
    /// <para>Date2 = Second Date</para>
    /// </summary>
    public string DateAdjustmentEarlier(DateTime Date1, DateTime Date2)
    {
        int result = DateTime.Compare(Date1, Date2);
        if (result < 0)
        {
            return Date1.ToShortDateString();
        }
        else if (result > 0)
        {
            return Date2.ToShortDateString();
        }
        else
        {
            return Date1.ToShortDateString();
        }
    }
    /// <summary>Find Later of two dates
    /// <para>Date1 = First Date</para>
    /// <para>Date2 = Second Date</para>
    /// </summary>
    public string DateAdjustmentLater(DateTime Date1, DateTime Date2)
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
    /// <summary>Find date occurances between two dates
    /// <para>Date1 = First Date</para>
    /// <para>Date2 = Second Date</para>
    /// <para>Day = Day for dates between</para>
    /// <para>Month = Month for dates between</para>
    /// </summary>
    public string DateAdjustmentDatesBetween(DateTime Date1, DateTime Date2, string Day, string Month)
    {
        if (Date1 <= Date2)
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
            if (dateIncrement < Date2)
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
    /// <summary>Get section of a date
    /// <para>Part = Part of a date</para>
    /// <para>Date = Date to get part of</para>
    /// </summary>
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