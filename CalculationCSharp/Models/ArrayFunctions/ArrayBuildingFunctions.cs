// Copyright (c) 2016 Project AIM
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
        /// <summary>Functions used when building/adjusting or splitting Arrays in the configuration Builder
        /// </summary>   

        /// <summary>Builds an array of data by splitting by ~ in the string of the array.
        /// <para>Input = string value to be split</para>
        /// <para>jCategory = the whole configuration which is required to do the variable replace</para>
        /// <para>GroupID = current Group ID</para>
        /// <para>ItemID = current row ID</para>
        /// </summary>   
        public string[] InputArrayBuilder(string Input, List<CategoryViewModel> jCategory, int GroupID, int ItemID)
        {
            CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
            if (!string.IsNullOrEmpty(Input))
            {
                string[] parts = null;
                //Split Array
                parts = Input.Split('~');
                string outputstring = null;
                string[] partsSplit = null;
                List<string> OutputList = new List<string>();
                //Loop through the array
                foreach (string date in parts)
                {
                    //Replace any value that has a variable attached to it
                    outputstring = Config.VariableReplace(jCategory, date, GroupID, ItemID);
                    if (outputstring.Contains("~"))
                    {
                        partsSplit = outputstring.Split('~');
                        foreach (string Output2 in partsSplit)
                        {
                            OutputList.Add(Output2);
                        }
                    }
                    //If not an array output the single value
                    else
                    {
                        OutputList.Add(outputstring);
                    }
                }
                return OutputList.ToArray();
            }
            return null;
        }
        /// <summary>Get's a part of an array, used in conjunction with a counter from a loop.
        /// <para>array = array to find the value</para>
        /// <para>Counter = point in the array to return</para>
        /// </summary>   
        public dynamic GetArrayPart(string[] array, int Counter)
        {
            if (array != null)
            {
                if (Counter >= array.Length)
                {
                    return array[array.GetUpperBound(0)];
                }
                else
                {
                    return array[Counter];
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>Get's max length of two array's.
        /// <para>Input1 = Array 1</para>
        /// <para>Input2 = Array 2</para>
        /// </summary>   
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