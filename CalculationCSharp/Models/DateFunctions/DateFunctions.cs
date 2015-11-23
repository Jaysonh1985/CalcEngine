using Microsoft.VisualBasic;
using System;

public class DateFunctions
{

	public int YearsMonthsBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive)
	{

		int months = MonthsBetween(StartDate, EndDate, false);

        return months / 12;
	}

	 public double YearsDaysBetween(System.DateTime StartDate, System.DateTime EndDate, bool inclusive, int daysperyear)
	{

		daysperyear = 365;
		int Years = YearsMonthsBetween(StartDate, EndDate, false);
		System.TimeSpan d;
        double days;
        d = EndDate.Subtract(StartDate);
        days = d.Days;
		//if (isLeapYear(StartDate, EndDate) == true) {
		//	daysperyear = 365;
		//}
		days = days / daysperyear;

        return days;

	}

	//bool isLeapYear(System.DateTime startdate, System.DateTime enddate)
	//{
	//	System.DateTime febcheck;

	//	febcheck = DateSerial(Year(enddate), 2, 28);
	//	if (startdate > febcheck | enddate < febcheck) {
	//		return false;
	//	}

	//	return true;
	//}

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

}