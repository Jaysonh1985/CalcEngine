// Copyright (c) 2016 Project AIM
using CalculationCSharp.Models.ArrayFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models
{
    public class MathsFunctions
    {
        public string Type { get; set; }
        public dynamic Number1 { get; set; }
        public dynamic Number2 { get; set; }
        public dynamic PeriodType { get; set; }
        public string Rounding { get; set; }
        public string RoundingType { get; set; }
        /// <summary>Outputs where Maths Functions function is used, includes the array builder.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public string Output (string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID, CategoryViewModel group)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            MathsFunctions parameters = (MathsFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(MathsFunctions));
            string[] Numbers1parts = null;
            string[] Numbers2parts = null;
            //Returns array
            Numbers1parts = ArrayBuilder.InputArrayBuilder(parameters.Number1, jCategory, GroupID, ItemID);
            Numbers2parts = ArrayBuilder.InputArrayBuilder(parameters.Number2, jCategory, GroupID, ItemID);
            string Output = null;
            Decimal OutputValue = 0;
            //Gets Max Length of array so loops through all values        
            int MaxLength = ArrayBuilder.GetMaxLength(Numbers1parts, Numbers2parts);
            int Counter = 0;
            //Loop through the array to calculate each value in array
            for (int i = 0; i < MaxLength; i++)
            {
                dynamic InputA = null;
                dynamic InputB = null;
                decimal InputADeci = 0;
                decimal InputBDeci = 0;
                //Gets the current array to use in the loop
                InputA = ArrayBuilder.GetArrayPart(Numbers1parts, Counter);
                InputB = ArrayBuilder.GetArrayPart(Numbers2parts, Counter);
                decimal.TryParse(InputA, out InputADeci);
                decimal.TryParse(InputB, out InputBDeci);
                //Calculates the value required
                OutputValue = Calculate(parameters, InputADeci, InputBDeci, ItemID, group);
                Output = Output + OutputValue + "~";
                Counter = Counter + 1;
            }            
            Output = Output.Remove(Output.Length - 1);
            return Convert.ToString(Output);
        }
        /// <summary>Runs the Maths Functions calculation.
        /// <para>parameters = JSON congifurations relating to this function</para>
        /// <para>InputADeci = value to be passed into the calculation</para>
        /// <para>InputBDeci = value to be passed into the calculation</para>
        /// </summary>
        public decimal Calculate(MathsFunctions parameters, dynamic InputADeci, dynamic InputBDeci, int ItemID, CategoryViewModel group)
        {
            MathematicalFunctions MathFunctions = new MathematicalFunctions();
            Decimal Output = 0;

            Rounding = Convert.ToString(parameters.Rounding);

            if (Rounding == null || Rounding == "")
            {
                Rounding = "2";
            }

            //Absolute
            if (parameters.Type == "Abs")
            {
                Output = MathFunctions.Abs(InputADeci);
            }
            //Add
            else if (parameters.Type == "Add")
            {
                Output = MathFunctions.Add(InputADeci, InputBDeci);
            }
            //Add Period
            else if (parameters.Type == "AddPeriod")
            {
                Output = MathFunctions.AddPeriod(InputADeci, InputBDeci, parameters.PeriodType);
                Rounding = "2";
                if (parameters.PeriodType == "YearsDays")
                {
                    Rounding = "3";
                }
            }
            //Ceiling
            else if (parameters.Type == "Ceiling")
            {
                Output = MathFunctions.Ceiling(InputADeci);
            }
            //Get decimal part only
            else if (parameters.Type == "DecimalPart")
            {
                Output = MathFunctions.DecimalPart(InputADeci);
            }
            //Divide
            else if (parameters.Type == "Divide")
            {
                Output = MathFunctions.Divide(InputADeci, InputBDeci);
            }
            //Floor to lowest integer
            else if (parameters.Type == "Floor")
            {
                Output = MathFunctions.Floor(InputADeci);
            }
            //Max of two values
            else if (parameters.Type == "Max")
            {
                Output = MathFunctions.Max(InputADeci, InputBDeci);
            }
            //Min of two values
            else if (parameters.Type == "Min")
            {
                Output = MathFunctions.Min(InputADeci, InputBDeci);
            }
            //Multiply of two values
            else if (parameters.Type == "Multiply")
            {
                Output = MathFunctions.Multiply(InputADeci, InputBDeci);
            }
            //Power of two value
            else if (parameters.Type == "Power")
            {
                Output = MathFunctions.Power(InputADeci, InputBDeci);
            }
            //Subtract two value
            else if (parameters.Type == "Subtract")
            {
                Output = MathFunctions.Subtract(InputADeci, InputBDeci);
            }
            //Subtract two value
            else if (parameters.Type == "SumDecimalAbove")
            {
                //var filterList = group.Functions.Where((l, index) => index >= (ItemID - InputADeci) && index < ItemID);
                //Output = 0;
                //foreach (var row  in filterList)
                //{
                //    if(row.Type == "Decimal" && row.Pass != "miss")
                //    {
                //        Output = Output + Convert.ToDecimal(row.Output);
                //    }                  
                //}             
            }
            //Subtract two period value
            else if (parameters.Type == "SubtractPeriod")
            {
                Output = MathFunctions.SubtractPeriod(InputADeci, InputBDeci, parameters.PeriodType);
                Rounding = "2";
                if (parameters.PeriodType == "YearsDays")
                {
                    Rounding = "3";
                }
            }
            //Truncate
            else if (parameters.Type == "Truncate")
            {
                Output = MathFunctions.Truncate(InputADeci);
            }
            return MathFunctions.Rounding(parameters.RoundingType, Rounding, Output);
        }
    }
}