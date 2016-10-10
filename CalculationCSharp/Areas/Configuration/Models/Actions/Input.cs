// Copyright (c) 2016 Project AIM
namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Input
    {
        public string Output (string Type, string Output)
        {
            if (Type == "Date")
            {
                if (Output != "" && Output != "0")
                {
                    return Output;
                }
                else
                {
                    return  "";
                }
            }
            else if (Type == "Decimal")
            {
                if (Output == "" || Output == null)
                {
                    return  "0";
                }
                else
                {
                    return Output;
                }
            }
            else
            {
                return Output;
            }         
        }
    }
}