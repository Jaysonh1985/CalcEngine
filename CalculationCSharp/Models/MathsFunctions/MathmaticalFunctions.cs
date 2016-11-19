// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

public class MathematicalFunctions
{
    /// <summary>Mathmatical Functions.
    /// </summary>

    /// <summary>Return Absolute value.
    /// <para>Number1 = Number to user</para>
    /// </summary>
    public decimal Abs(decimal Number1)
	{
        return Math.Abs(Number1);
	}
    /// <summary>Ceiling return the round up value.
    /// <para>Number1 = Number to user</para>
    /// </summary>
    public decimal Ceiling(decimal Number1)
    {
        return Math.Ceiling(Number1);
    }
    /// <summary>Floor return the round down value.
    /// <para>Number1 = Number to user</para>
    /// </summary>
    public decimal Floor(decimal Number1)
    {
        return Math.Floor(Number1);
    }
    /// <summary>Find Max value of two
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal Max(decimal Number1, Decimal Number2)
    {
        return Math.Max(Number1, Number2);
    }
    /// <summary>Find Min value of two
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal Min(decimal Number1, Decimal Number2)
    {
        return Math.Min(Number1, Number2);
    }
    /// <summary>Truncate the number
    /// <para>Number1 = Number to user</para>
    /// </summary>
    public decimal Truncate(decimal Number1)
    {
        return Math.Truncate(Number1);
    }
    /// <summary>Return Decimal Part of number
    /// <para>Number1 = Number to user</para>
    /// </summary>
    public decimal DecimalPart(decimal Number1)
    {     
        return Number1 - Math.Truncate(Number1);
    }
    /// <summary>Add two numbers together
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal Add(decimal Number1, decimal Number2)
    {
        return Number1 + Number2;
    }
    /// <summary>Add two Periods together
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal AddPeriod(decimal Number1, decimal Number2, string PeriodType)
    {
        decimal Number1Decimal = Number1 - Math.Truncate(Number1);
        decimal Number2Decimal = Number2 - Math.Truncate(Number2);

        decimal Number1Int = Math.Floor(Number1);
        decimal Number2Int = Math.Floor(Number2);

        decimal OutputInt = Number1Int + Number2Int;
        decimal OutputDecimal = Number1Decimal + Number2Decimal;

        if(PeriodType == "YearsDays")
        {
            if (OutputDecimal >= Convert.ToDecimal(0.365))
            {
                OutputInt = OutputInt + 1;
                OutputDecimal = Number1Decimal + Number2Decimal - Convert.ToDecimal(0.365);
            }
        }
        else if(PeriodType == "YearsMonths")
        {
            if (OutputDecimal >= Convert.ToDecimal(0.12))
            {
                OutputInt = OutputInt + 1;
                OutputDecimal = Number1Decimal + Number2Decimal - Convert.ToDecimal(0.12);
            }
        }

        return OutputInt + OutputDecimal;
    }
    /// <summary>Subtract two numbers
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal Subtract(decimal Number1, decimal Number2)
    {
        return Number1 - Number2;
    }
    /// <summary>Subtract two Periods together
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal SubtractPeriod(decimal Number1, decimal Number2, string PeriodType)
    {
        decimal Number1Decimal = Number1 - Math.Truncate(Number1);
        decimal Number2Decimal = Number2 - Math.Truncate(Number2);

        decimal Number1Int = Math.Floor(Number1);
        decimal Number2Int = Math.Floor(Number2);

        decimal OutputInt = Number1Int - Number2Int;
        decimal OutputDecimal = Number1Decimal - Number2Decimal;

        if (PeriodType == "YearsDays")
        {
            if (OutputDecimal >= Convert.ToDecimal(0.365))
            {
                OutputInt = OutputInt + 1;
                OutputDecimal = Number1Decimal + Number2Decimal - Convert.ToDecimal(0.365);
            }
        }
        else if (PeriodType == "YearsMonths")
        {
            if (OutputDecimal >= Convert.ToDecimal(0.12))
            {
                OutputInt = OutputInt + 1;
                OutputDecimal = Number1Decimal + Number2Decimal - Convert.ToDecimal(0.12);
            }
        }

        return OutputInt + OutputDecimal;
    }
    /// <summary>Divide two numbers
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal Divide(decimal Number1, decimal Number2)
    {
        try
        {
            return Number1 / Number2;
        }
        catch (Exception)
        {
            return 0;
        }
    }
    /// <summary>Multiple two numbers
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal Multiply(decimal Number1, decimal Number2)
    {
        return  Number1 * Number2;
    }
    /// <summary>Power two numbers
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal Power(decimal Number1, decimal Number2)
    {
        return Convert.ToDecimal(Math.Pow(Convert.ToDouble(Number1), Convert.ToDouble(Number2)));
    }
    /// <summary>Rounding
    /// <para>RoundingType = Type of rounding to apply</para>
    /// <para>Rounding = value of rounding in decimal places</para>
    /// <para>Output = Output</para>
    /// </summary>
    public decimal Rounding (string RoundingType, string Rounding, decimal Output)
    {
        if (RoundingType == "up")
        {
            //decimal value = RoundingRoundUp(Rounding, Output);
            return RoundingRoundUp(Rounding, Output);
        }
        else if (RoundingType == "down")
        {
            if (Convert.ToInt16(Rounding) == 0)
            {
                return Math.Truncate(Output);
            }
            else
            {
                return RoundingRoundDown(Rounding, Output); 
            }
        }
        else
        {
            if(Rounding != "")
            {
                return Convert.ToDecimal(RoundingDecimalPlaces(Rounding, Math.Round(Output, Convert.ToInt16(Rounding))));
            }
            else
            {
                return Convert.ToDecimal(RoundingDecimalPlaces(Rounding, Math.Round(Output, 2)));
            }
        }
    }
    /// <summary>RoundingRoundUp Function 
    /// <para>RoundingType = Type of rounding to apply</para>
    /// <para>Value = Value to apply rounding type too</para>
    /// </summary>
    public decimal RoundingRoundUp(string Rounding, decimal Value)
    {
        if (Rounding == "1")
        {
            return Math.Round(Math.Ceiling(Value * 100) / 100, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "2")
        {
            return Math.Round(Math.Ceiling(Value * 100) / 100, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "3")
        {
            return Math.Round(Math.Ceiling(Value * 1000) / 1000, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "4")
        {
            return Math.Round(Math.Ceiling(Value * 10000) / 10000, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "5")
        {
            return Math.Round(Math.Ceiling(Value * 100000) / 100000, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "6")
        {
            return Math.Round(Math.Ceiling(Value * 1000000) / 1000000, Convert.ToInt16(Rounding));
        }
        else
        {
            return Math.Round(Math.Ceiling(Value * 100) / 100, Convert.ToInt16(Rounding));
        }
    }

    /// <summary>RoundingRoundDown Function 
    /// <para>RoundingType = Type of rounding to apply</para>
    /// <para>Value = Value to apply rounding type too</para>
    /// </summary>
    public decimal RoundingRoundDown(string Rounding, decimal Value)
    {
        if (Rounding == "1")
        {
            return Math.Round(Math.Floor(Value * 100) / 100, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "2")
        {
            return Math.Round(Math.Floor(Value * 100) / 100, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "3")
        {
            return Math.Round(Math.Floor(Value * 1000) / 1000, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "4")
        {
            return Math.Round(Math.Floor(Value * 10000) / 10000, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "5")
        {
            return Math.Round(Math.Floor(Value * 100000) / 100000, Convert.ToInt16(Rounding));
        }
        else if (Rounding == "6")
        {
            return Math.Round(Math.Floor(Value * 1000000) / 1000000, Convert.ToInt16(Rounding));
        }
        else
        {
            return Math.Round(Math.Floor(Value * 100) / 100, Convert.ToInt16(Rounding));
        }
    }
    /// <summary>RoundingDecimalPlaces Function, this function displays the decimal strings on the builder to the relevant decimal places
    /// <para>RoundingType = Type of rounding to apply</para>
    /// <para>Value = Value to apply rounding type too</para>
    /// </summary>
    public string RoundingDecimalPlaces(string Rounding, decimal Value)
    {

        if(Rounding == "1")
        {
            return Value.ToString("0.0");
        }
        else if (Rounding == "2")
        {
            return Value.ToString("0.00");
        }
        else if (Rounding == "3")
        {
            return Value.ToString("0.000");
        }
        else if (Rounding == "4")
        {
            return Value.ToString("0.0000");
        }
        else if (Rounding == "5")
        {
            return Value.ToString("0.00000");
        }
        else if (Rounding == "6")
        {
            return Value.ToString("0.000000");
        }
        else
        {
            return Convert.ToString(Value);
        }

    }
}