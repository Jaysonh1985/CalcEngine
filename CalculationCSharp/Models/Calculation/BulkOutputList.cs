using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Models.Calculation
{
    public class BulkOutputListBuilder
    {
        public List<List<string>> BulkOutput (List<List<OutputListGroup>> BulkOutputList)
        {
            bool isNameDone = false;
            //Iterate through data list collection
            List<List<string>> propNames = new List<List<string>>();
            List<string> propValues = new List<string>();
            //Loop to output the values in the csv
            int LoopCounter = 0;
            //Create CSV of the output results
            foreach (var item in BulkOutputList)
            {
                LoopCounter = 0;
                foreach (var list in item)
                {
                    //Sets the Group in the output
                    //Iterate through property collection
                    foreach (var prop in list.Output)
                    {
                        //Sets the row label
                        if (isNameDone == false)
                        {
                            LoopCounter = LoopCounter + 1;
                            if (prop.SubOutput != null)
                            {   
                                foreach(var itemSubOutput in prop.SubOutput)
                                {
                                    foreach(var propSubOutput in itemSubOutput.Output)
                                    {
                                        propValues.Add("Sub Output");
                                        propValues.Add(itemSubOutput.Group);
                                        propValues.Add(propSubOutput.Field);
                                        propValues.Add(propSubOutput.Value);
                                        propNames.Add(propValues);
                                        propValues = new List<string>();
                                    }
                                }
                            };
                            propValues.Add("");
                            propValues.Add(list.Group);
                            propValues.Add(prop.Field);
                            propValues.Add(prop.Value);
                            propNames.Add(propValues);
                            propValues = new List<string>();
                        }
                        //sets the row value
                        else
                        {
                            propValues.Add(prop.Value);
                            propNames[LoopCounter].Add(prop.Value);
                            propValues = new List<string>();
                            LoopCounter = LoopCounter + 1;
                        }
                    }
                }
                isNameDone = true;
            }
            return propNames;
        }
    }
}