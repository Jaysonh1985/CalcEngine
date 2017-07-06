using CalculationCSharp.Models.ArrayFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Return
    {
        public string Type { get; set; }
        public dynamic Variable { get; set; }
        public string Name { get; set; }
        public dynamic Output { get; set; }

        /// <summary>Return where function is being built this is the value that should be passed back to the main calculation.
        /// <para>jparameters = JSON congifurations relating to this function</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public string Output1(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            Return parameters = (Return)javaScriptSerializ­er.Deserialize(jparameters, typeof(Return));
            string[] Return1parts = null;
            //Returns Array
            Return1parts = ArrayBuilder.InputArrayBuilder(parameters.Variable, jCategory, GroupID, ItemID);
            string Output = null;
            //Loop through the array to calculate each value in array
            foreach (string part in Return1parts)
            {
                dynamic InputA = Config.VariableReplace(jCategory, part, GroupID, ItemID);
                Output = Output + Convert.ToString(InputA) + "~";
            }
            Output = Output.Remove(Output.Length - 1);
            return Convert.ToString(Output);
        }

    }
}