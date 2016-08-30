using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

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

        public string Output (string jparameters, List<CategoryViewModel> jCategory, int GroupID, int ItemID, string itemType)
        {
            LookupFunctions FactorFunctions = new LookupFunctions();
            Factors parameters = (Factors)javaScriptSerializ­er.Deserialize(jparameters, typeof(Factors));
            dynamic InputA = Config.VariableReplace(jCategory, parameters.LookupValue, GroupID, ItemID);
            dynamic InputB = Config.VariableReplace(jCategory, parameters.RowMatchValue, GroupID, ItemID);
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
                    return Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 1, parameters.ColumnNo));
                }
                else if (parameters.LookupType == "Decimal")
                {
                    decimal LookupValue;
                    decimal.TryParse(InputA, out LookupValue);

                    return Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 1, parameters.ColumnNo));
                }
                else
                {
                    string LookupValue;
                    LookupValue = InputA;
                    return Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 2, parameters.ColumnNo));
                }

                itemType = parameters.OutputType;

            }
            else
            {
                itemType = parameters.OutputType;

                if (parameters.OutputType == "Decimal")
                {
                    return "0";
                }
                else
                {
                    return  "";
                }
            }
        }
    }
}