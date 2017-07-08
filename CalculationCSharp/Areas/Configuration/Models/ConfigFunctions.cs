// Copyright (c) 2016 Project AIM
using System.Collections.Generic;
using System.Linq;


namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ConfigFunctions
    {

        /// <summary>Variable Replace function takes the string variable and finds the value associated with this, the last row above the current row
        /// <para>CategoryViewModel = category view model which is the builder this is required so that the variables can be found </para>
        /// <para>Input = variable value that requires replacement</para>
        /// <para>colID = Current column ID to define the current row and column</para>
        /// <para>rowID = Current row ID to define the current row and column</para>
        /// </summary>
        public dynamic VariableReplace(List<CategoryViewModel> CategoryViewModel, string Input,int colID, int rowID)
        {
            dynamic element = null;
            dynamic Output = null;
            dynamic elementType = null;
            //Loop around whole configuration
            foreach (var item in CategoryViewModel)
            {
                List<ConfigViewModel> ConfigViewModel = new List<ConfigViewModel>();
                ConfigViewModel = item.Functions;
                //Find in columns above
                if(item.ID < colID)
                {
                    foreach(var row in item.Functions){
                        if (row.Function == "Function")
                        {
                            foreach (var FunctionRow in row.FunctionOutput)
                            {
                                if (FunctionRow.Name == Input)
                                {
                                    element = FunctionRow.Output;
                                };

                            };
                        }
                        else
                        {
                            if(row.Name == Input)
                            {
                                element = row.Output;
                            }
                        };
                    };
                }
                //Find in current column
                else if (item.ID == colID)
                {
                    int counter = 0;
                    foreach (var row in item.Functions)
                    {
                        if (counter == rowID) break;
                        if (row.Function == "Function")
                        {
                            foreach (var FunctionRow in row.FunctionOutput)
                            {
                                if (FunctionRow.Name == Input)
                                {
                                    element = FunctionRow.Output;
                                };

                            };
                        }
                        else
                        {
                            if (row.Name == Input)
                            {
                                element = row.Output;
                            }
                        };
                        counter++;
                    };
                }
                //Set the value of the variable replace if available this saves it and can be overwrriten if a further one is available.
                if(element != null)
                {
                    Output = element; 
                }
            }
            //If null then set the values to either return the default value or pass back the variable name
            if (Output == null)
            {
                if(elementType != null)
                {
                    if(elementType.Type == "Decimal")
                    {
                        return 0;
                    }
                    else if(elementType.Type == "Date")
                    {
                        return "";
                    }
                    else
                    {
                        return Input;
                    }
                }

                return Input;
            }
            else
            {
                return Output;
            }
        }
    }
}