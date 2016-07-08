using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.Threading;

public class DateFunctions
{

	public int YearsMonthsBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive)
	{
        int months = MonthsBetween(StartDate, EndDate, false);

        return months / 12;
	}

	 public double YearsDaysBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive, double daysperyear)
	{

        System.TimeSpan d;
        double days;
        d = EndDate.Subtract(StartDate);
        days = d.Days;

        double Years = Math.Floor(days / daysperyear);

		days = (days / daysperyear) - Years;
        days = Math.Floor(days * daysperyear);
        days = Math.Round((days / 1000),3,MidpointRounding.AwayFromZero);
        days = Years + days;
        return days;

	}

	public int MonthsBetween(System.DateTime StartDate, System.DateTime EndDate, bool roundup)
	{

		int Months;
		int i = 1;
		if (EndDate < StartDate) {
			i = i - 1;

			StartDate = EndDate;
			EndDate = StartDate;
		}

		Months = ((EndDate.Year * 12) + EndDate.Month) - ((StartDate.Month * 12) + StartDate.Month);

		if (EndDate.Day < StartDate.Day) {
			Months = Months - 1;
		}

		if ((roundup == true & EndDate.Day != StartDate.Day)) {
			Months = Months + 1;
		}

		return Months * i;

	}

    public DateTime? DateAdjustment(string Type, String DateA, String DateB, string PeriodType, decimal Period, string Adjustment, string Day)
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

                return AdjDate;
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

                return AdjDate;
            }

        }
        else if (Type == "Adjust")
        {
            string date = Day;
            string[] dateTokens = date.Split('/');

            string strDay = dateTokens[0];
            string strMonth = dateTokens[1];

            int intDay = Convert.ToInt32(strDay);
            int intMonth = Convert.ToInt32(strMonth);

            DateTime Value = new DateTime(Date1.Year, intMonth, intDay);

            if(Adjustment == "Less")
            {
                if (Value <= Date1)
                {
                    return Value;
                }
                else
                {
                    Value = new DateTime(Date1.Year - 1, intMonth, intDay);

                    return Value;
                }
            }
            else
            {
                if (Value >= Date1)
                {
                    return Value;
                }
                else
                {
                    Value = new DateTime(Date1.Year + 1, intMonth, intDay);

                    return Value;
                }
            }





        }

        else if (Type == "Earlier")
        {
            int result = DateTime.Compare(Date1, Date2);

            if(result < 0)
            {
                return Date1;
            }
            else if(result > 0)
            {
                return Date2;
            }
            else
            {
                return Date1;
            }

        }
        else if (Type == "Later")
        {
            int result = DateTime.Compare(Date1, Date2);

            if (result < 0)
            {
                return Date2;
            }
            else if (result > 0)
            {
                return Date1;
            }
            else
            {
                return Date1;
            }

        }
        return Convert.ToDateTime("01/01/1900"); 
    }

}