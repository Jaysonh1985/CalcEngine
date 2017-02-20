using CalculationCSharp.Models.ArrayFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Function
    {
        public int ID { get; set; }
        public string Scheme { get; set; }
        public string FunctionName { get; set; }
        public List<ConfigViewModel> Input { get; set; }

        /// <summary>Output Factor function is used, includes the array builder.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        public LookupFunctions FactorFunctions = new LookupFunctions();
        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID, dynamic variable, string DataType)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            string[] Parts = null;
            //Returns Array
            Parts = ArrayBuilder.InputArrayBuilder(variable, jCategory, GroupID, ItemID);
            string Output = null;
            //Loop through the array to calculate each value in array
            foreach (string part in Parts)
            {
                dynamic InputA = Config.VariableReplace(jCategory, part, GroupID, ItemID);
                if(DataType == "Date")
                {
                    DateTime Date1;
                    DateTime.TryParse(InputA, out Date1);
                    Output = Output + Convert.ToString(Date1.ToShortDateString()) + "~";
                }
                else if(DataType == "Decimal")
                {
                    Int16 Int1;
                    Int16.TryParse(InputA, out Int1);
                    Output = Output + Convert.ToString(Int1) + "~";
                }
                else
                {
                    Output = Output + Convert.ToString(InputA) + "~";
                }
            }
            Output = Output.Remove(Output.Length - 1);
            return Convert.ToString(Output);
        }
    }
}