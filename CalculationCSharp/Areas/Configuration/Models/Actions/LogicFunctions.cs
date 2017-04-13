// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class LogicFunctions
    {
        public dynamic Input1 { get; set; }
        public dynamic Input2 { get; set; }
        public string LogicInd { get; set; }
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }

        /// <summary>Outputs where Logic is set on the row .
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>bit = the logic to set</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>
        public string Output(List<CategoryViewModel> jCategory, LogicFunctions bit, int GroupID, int ItemID)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
            dynamic InputA = Config.VariableReplace(jCategory, bit.Input1, GroupID, ItemID);
            dynamic InputB = Config.VariableReplace(jCategory, bit.Input2, GroupID, ItemID);
            dynamic TrueVal = Config.VariableReplace(jCategory, bit.TrueValue, GroupID, ItemID);
            dynamic FalseVal = Config.VariableReplace(jCategory, bit.FalseValue, GroupID, ItemID);
            //Parses Decimals
            bool InputADeciSucceeded;
            bool InputBDeciSucceeded;
            decimal InputADeci;
            decimal InputBDeci;
            InputADeciSucceeded = decimal.TryParse(InputA, out InputADeci);
            InputBDeciSucceeded = decimal.TryParse(InputB, out InputBDeci);
            //Parses Dates
            bool InputADateSucceeded;
            bool InputBDateSucceeded;
            DateTime InputADate;
            DateTime InputBDate;
            InputADateSucceeded = DateTime.TryParse(InputA, out InputADate);
            InputBDateSucceeded = DateTime.TryParse(InputB, out InputBDate);
            object result = null;
            MethodInfo addMethod = this.GetType().GetMethod(bit.LogicInd);
            //Builds the string to calculate the logis
            if (InputADeciSucceeded == true && InputBDeciSucceeded == true)
            {               
                result = addMethod.Invoke(this, new object[] { InputADeci, InputBDeci });
            }
            else if (InputADeciSucceeded == true && InputBDeciSucceeded == false)
            {
                result = addMethod.Invoke(this, new object[] { InputADeci, InputB });
            }
            else if (InputADeciSucceeded == false && InputBDeciSucceeded == true)
            {
                result = addMethod.Invoke(this, new object[] { InputA, InputBDeci });
            }
            else if (InputADateSucceeded == true && InputBDateSucceeded == true)
            {
                result = addMethod.Invoke(this, new object[] { InputADate, InputBDate });
            }
            else if (InputADateSucceeded == true && InputBDateSucceeded == false)
            {
                result = addMethod.Invoke(this, new object[] { InputADate, InputBDate });               
            }
            else if (InputADateSucceeded == false && InputBDateSucceeded == true)
            {
                result = addMethod.Invoke(this, new object[] { InputADate, InputBDate });
            }
            else
            {
                result = addMethod.Invoke(this, new object[] { Convert.ToString(InputA), Convert.ToString(InputB)});
            }
            if(Convert.ToBoolean(result) == true)
            {
                return Convert.ToString(TrueVal);
            }
            else
            {
                return Convert.ToString(FalseVal);
            }
        }
        public bool NotEqual(dynamic InputA, dynamic InputB)
        {
            return InputA != InputB;
        }
        public bool Equal(dynamic InputA, dynamic InputB)
        {
            return InputA == InputB;
        }
        public bool Greater(dynamic InputA, dynamic InputB)
        {
            return InputA > InputB;
        }
        public bool GreaterEqual(dynamic InputA, dynamic InputB)
        {
            return InputA >= InputB;
        }
        public bool Less(dynamic InputA, dynamic InputB)
        {
            return InputA < InputB;
        }
        public bool LessEqual(dynamic InputA, dynamic InputB)
        {
            return InputA <= InputB;
        }
    }
}