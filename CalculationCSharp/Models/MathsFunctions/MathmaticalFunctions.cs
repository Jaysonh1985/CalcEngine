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
}