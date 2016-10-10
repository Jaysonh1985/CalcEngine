// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Logic
    {
        public string Bracket1 { get; set; }
        public dynamic Input1 { get; set; }
        public dynamic Input2 { get; set; }
        public string LogicInd { get; set; }
        public string Bracket2 { get; set; }
        public string Operator { get; set; }

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output(List<CategoryViewModel> jCategory, Logic bit, int GroupID, int ItemID)
        {
            dynamic InputA = Config.VariableReplace(jCategory, bit.Input1, GroupID, ItemID);
            dynamic InputB = Config.VariableReplace(jCategory, bit.Input2, GroupID, ItemID);
            string Logic = null;

            if (bit.LogicInd == "NotEqual")
            {
                Logic = "<>";
            }
            else if (bit.LogicInd == "Greater")
            {
                Logic = ">";
            }
            else if (bit.LogicInd == "GreaterEqual")
            {
                Logic = ">=";
            }
            else if (bit.LogicInd == "Less")
            {
                Logic = "<";
            }
            else if (bit.LogicInd == "LessEqual")
            {
                Logic = "<=";
            }
            else
            {
                Logic = bit.LogicInd;
            }

            bool InputADeciSucceeded;
            bool InputBDeciSucceeded;
            decimal InputADeci;
            decimal InputBDeci;
            InputADeciSucceeded = decimal.TryParse(InputA, out InputADeci);
            InputBDeciSucceeded = decimal.TryParse(InputB, out InputBDeci);

            bool InputADateSucceeded;
            bool InputBDateSucceeded;
            DateTime InputADate;
            DateTime InputBDate;
            InputADateSucceeded = DateTime.TryParse(InputA, out InputADate);
            InputBDateSucceeded = DateTime.TryParse(InputB, out InputBDate);

            if (InputADeciSucceeded == true && InputBDeciSucceeded == true)
            {
                return "if(" + InputADeci + Logic + InputBDeci + ",true,false)";
            }
            else if (InputADeciSucceeded == true && InputBDeciSucceeded == false)
            {
                return "if(" + InputADeci + Logic + "'" + InputBDeci + "'" + ",true,false)";
            }
            else if (InputADeciSucceeded == false && InputBDeciSucceeded == true)
            {
                return "if(" + "'" + InputADeci + "'" + Logic + InputBDeci + ",true,false)";
            }
            else if (InputADateSucceeded == true && InputBDateSucceeded == true)
            {
                return "if(" + "'" + InputADate + "'" + Logic + "'" + InputBDate + "'" + ",true,false)";
            }
            else if (InputADateSucceeded == true && InputBDateSucceeded == false)
            {
                return "if(" + InputADate + Logic + "'" + InputBDate + "'" + ",true,false)";
            }
            else if (InputADateSucceeded == false && InputBDateSucceeded == true)
            {
                return "if(" + "'" + InputADate + "'" + Logic + InputBDate + ",true,false)";
            }
            else
            {
                string inputA = Convert.ToString(InputA);
                string inputB = Convert.ToString(InputB);
                return "if(" + "'" + inputA + "'" + Logic + "'" + inputB + "'" + ",true,false)";
            }
        }
    }
}