using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

public class MathematicalFunctions
{

	public decimal Abs(decimal Number1)
	{
        return Math.Abs(Number1);
	}

    public decimal Ceiling(decimal Number1)
    {
        return Math.Ceiling(Number1);
    }

    public decimal Floor(decimal Number1)
    {
        return Math.Floor(Number1);
    }

    public decimal Max(decimal Number1, Decimal Number2)
    {
        return Math.Max(Number1, Number2);
    }

    public decimal Min(decimal Number1, Decimal Number2)
    {
        return Math.Min(Number1, Number2);
    }

    public decimal Truncate(decimal Number1)
    {
        return Math.Truncate(Number1);
    }
    public decimal DecimalPart(decimal Number1)
    {     
        return Number1 - Math.Truncate(Number1);
    }

    public decimal Add(decimal Number1, decimal Number2)
    {
        return Number1 + Number2;
    }

    public decimal Subtract(decimal Number1, decimal Number2)
    {
        return Number1 - Number2;
    }

    public decimal Divide(decimal Number1, decimal Number2)
    {
        return Number1 / Number2;
    }
    public decimal Multiply(decimal Number1, decimal Number2)
    {
        return  Number1 * Number2;
    }
    public decimal Power(decimal Number1, decimal Number2)
    {
        return Convert.ToDecimal(Math.Pow(Convert.ToDouble(Number1), Convert.ToDouble(Number2)));
    }

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