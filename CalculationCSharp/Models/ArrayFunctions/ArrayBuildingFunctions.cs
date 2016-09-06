using CalculationCSharp.Areas.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Models.ArrayFunctions
{
    public class ArrayBuildingFunctions
    {
        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
        public List<string> InputArrayBuilder(string Input, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            if (!string.IsNullOrEmpty(Input))
            {
                string[] parts = null;
                parts = Input.Split(',');
                string outputstring = null;
                string[] partsSplit = null;
                List<string> OutputList = new List<string>();

                foreach (string date in parts)
                {
                    outputstring = Config.VariableReplace(jCategory, date, GroupID, ItemID);
                    if (outputstring.Contains(","))
                    {
                        partsSplit = outputstring.Split(',');

                        foreach (string date2 in partsSplit)
                        {
                            OutputList.Add(date2);
                        }
                    }
                    else
                    {
                        OutputList.Add(outputstring);
                    }

                }

                return OutputList;
            }
            return null;
        }
        public int GetMaxLength(string[] Input1, string[] Input2)
        {
            int Length1 = 0;
            if (Input1 != null)
            {
                Length1 = Input1.Length;
            }

            int Length2 = 0;
            if (Input2 != null)
            {
                Length2 = Input2.Length;
            }

            return Math.Max(Length1, Length2);
        }
    }
}