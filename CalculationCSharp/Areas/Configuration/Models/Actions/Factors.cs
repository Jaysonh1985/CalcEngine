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

        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public string Output(string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID, string itemType)
        {
            LookupFunctions FactorFunctions = new LookupFunctions();
            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
            Factors parameters = (Factors)javaScriptSerializ­er.Deserialize(jparameters, typeof(Factors));
            List<string> D1parts = null;
            List<string> D2parts = null;
            string[] LookupValueparts = null;
            string[] RowMatchValueparts = null;

            D1parts = ArrayBuilder.InputArrayBuilder(parameters.LookupValue, jCategory, GroupID, ItemID);
            D2parts = ArrayBuilder.InputArrayBuilder(parameters.RowMatchValue, jCategory, GroupID, ItemID);

            if (D1parts != null)
            {
                LookupValueparts = D1parts.ToArray();
            }
            if (D2parts != null)
            {
                RowMatchValueparts = D2parts.ToArray();
            }

            string Output = null;
            int Counter = 0;
            string OutputValue = null;

            int MaxLength = ArrayBuilder.GetMaxLength(LookupValueparts, RowMatchValueparts);

            for (int i = 0; i < MaxLength; i++)
            {

                dynamic InputA = null;
                dynamic InputB = null;

                if (LookupValueparts != null)
                {
                    if (Counter >= LookupValueparts.Length)
                    {
                        InputA = LookupValueparts[LookupValueparts.GetUpperBound(0)];
                    }
                    else
                    {
                        InputA = LookupValueparts[Counter];
                    }

                }
                if (RowMatchValueparts != null)
                {
                    if (Counter >= RowMatchValueparts.Length)
                    {
                        InputB = RowMatchValueparts[RowMatchValueparts.GetUpperBound(0)];
                    }
                    else
                    {
                        InputB = RowMatchValueparts[Counter];
                    }
                }
                
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
                            OutputValue = Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 1, parameters.ColumnNo));
                        }
                        else if (parameters.LookupType == "Decimal")
                        {
                            decimal LookupValue;
                            decimal.TryParse(InputA, out LookupValue);
                            OutputValue = Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 1, parameters.ColumnNo));
                        }
                        else
                        {
                            string LookupValue;
                            LookupValue = InputA;
                            OutputValue = Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 2, parameters.ColumnNo));
                        }
                    }
                    else
                    {
                        itemType = parameters.OutputType;
                        if (parameters.OutputType == "Decimal")
                        {
                            OutputValue = "0";
                        }
                        else
                        {
                            OutputValue = "";
                        }
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
    }
}
