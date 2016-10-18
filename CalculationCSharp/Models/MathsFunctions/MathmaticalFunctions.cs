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
    /// <summary>Subtract two numbers
    /// <para>Number1 = Number to user</para>
    /// <para>Number2 = Number to user</para>
    /// </summary>
    public decimal Subtract(decimal Number1, decimal Number2)
    {
        return Number1 - Number2;
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
            return  Math.Round(Math.Ceiling(Output * 100) / 100, Convert.ToInt16(Rounding));
        }
        else if (RoundingType == "down")
        {
            if (Convert.ToInt16(Rounding) == 0)
            {
                return Math.Truncate(Output);
            }
            else
            {
                return Math.Round(Math.Floor(Output * 100) / 100, Convert.ToInt16(Rounding));
            }
        }
        else
        {
            if(Rounding != "")
            {
                return Math.Round(Output, Convert.ToInt16(Rounding));
            }
            else
            {
                return Math.Round(Output, 2);
            }
        }
    }
}