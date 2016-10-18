// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using CalculationCSharp.Models.ArrayFunctions;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class Factors
    {
        public string TableName { get; set; }
        public string LookupType { get; set; }
        public dynamic LookupValue { get; set; }
        public string OutputType { get; set; }
        public bool RowMatch { get; set; }
        public string RowMatchLookupType { get; set; }
        public int RowMatchRowNo { get; set; }
        public string RowMatchValue { get; set; }
        public int ColumnNo { get; set; }
        public bool Interpolate { get; set; }
        /// <summary>Output Factor function is used, includes the array builder.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        public LookupFunctions FactorFunctions = new LookupFunctions();
        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            Factors parameters = (Factors)javaScriptSerializ­er.Deserialize(jparameters, typeof(Factors));
            string[] LookupValueparts = null;
            string[] RowMatchValueparts = null;
            //Returns array
            LookupValueparts = ArrayBuilder.InputArrayBuilder(parameters.LookupValue, jCategory, GroupID, ItemID);
            RowMatchValueparts = ArrayBuilder.InputArrayBuilder(parameters.RowMatchValue, jCategory, GroupID, ItemID);
            string Output = null;
            int Counter = 0;
            string OutputValue = null;
            int MaxLength = ArrayBuilder.GetMaxLength(LookupValueparts, RowMatchValueparts);
            //Loop through the array to calculate each value in array
            for (int i = 0; i < MaxLength; i++)
            {
                dynamic InputA = null;
                dynamic InputB = null;
                //Gets the current array to use in the loop
                InputA = ArrayBuilder.GetArrayPart(LookupValueparts, Counter);
                InputB = ArrayBuilder.GetArrayPart(RowMatchValueparts, Counter);
                if(parameters.Interpolate == true)
                {
                    OutputValue = Interpolation(jparameters, InputA, InputB);
                }
                else
                {
                    OutputValue = Calculate(jparameters, InputA, InputB);
                }          
                Output = Output + OutputValue + "~";
                Counter = Counter + 1;
            }
            if (Output != null)
            {
                Output = Output.Remove(Output.Length - 1);
            }
            return Output;
        }

        /// <summary>Calculates a simple look up without interpolation.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>InputA = Lookup value vertically</para>
        /// <para>InputA = Lookup value Horizontally</para>
        /// </summary>
        public string Calculate(string jparameters, dynamic InputA, dynamic InputB)
        {
            Factors parameters = (Factors)javaScriptSerializ­er.Deserialize(jparameters, typeof(Factors));
            //Rowmatch does a match on a selected row if looking for a value horizontally
            if (parameters.RowMatch == true)
            {
                parameters.ColumnNo = FactorFunctions.CSVColumnNumber(parameters.TableName, parameters.RowMatchRowNo, InputB);
            }
           
            if (parameters.ColumnNo > 0)
            {
                if (parameters.LookupType == "Date")
                {
                    DateTime LookupValue;
                    DateTime.TryParse(InputA, out LookupValue);
                    return Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 1, parameters.ColumnNo));
                }
                else if (parameters.LookupType == "Decimal")
                {
                    decimal LookupValue;
                    decimal.TryParse(InputA, out LookupValue);
                    return Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 1, parameters.ColumnNo));
                }
                else
                {
                    string LookupValue;
                    LookupValue = InputA;
                    return Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 2, parameters.ColumnNo));
                }
            }
            else
            {
                if (parameters.OutputType == "Decimal")
                {
                    return "0";
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>Provides an interpolation calculation based on previous two values.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>InputA = Lookup value vertically</para>
        /// <para>InputA = Lookup value Horizontally</para>
        /// </summary>
        public string Interpolation (string jparameters, dynamic InputA, dynamic InputB)
        {
            string Year = Convert.ToString(Math.Floor(Convert.ToDecimal(InputA)));
            string Year1 = Convert.ToString(Convert.ToDecimal(Year) + 1);
            string Factor1 = Calculate(jparameters, Year, InputB);
            string Factor2 = Calculate(jparameters, Year1, InputB);
            dynamic Months = (Convert.ToDecimal(InputA) - Convert.ToDecimal(Year)) * 100;
            Decimal Output = Math.Round(((Convert.ToDecimal(Factor2) - Convert.ToDecimal(Factor1)) / 12 * Months) + Convert.ToDecimal(Factor1),6);
            return Convert.ToString(Output);
        }
    }
}
