﻿using CalculationCSharp.Areas.Configuration.Models;
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using NCalc;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Web;
using CalculationCSharp.Models.Calculation;
using System.Linq;

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Calculate
    {
        public List<OutputList> OutputList = new List<OutputList>();
        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        public List<CategoryViewModel> jCategory = new List<CategoryViewModel>();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();

        public List<CategoryViewModel> DebugResults(List<CategoryViewModel> jCategory)
        {
            Calculate Calculate = new Calculate();
            Calculate.CalculateAction(jCategory);
            return jCategory;
        }
        public List<OutputListGroup> OutputResults(List<CategoryViewModel> jCategory)
        {
            Calculate Calculate = new Calculate();
            Calculate.CalculateAction(jCategory);

            var OutputListGroupOrdered = Calculate.OutputList.GroupBy(employees => employees.Group);
            List<OutputListGroup> List = new List<OutputListGroup>();
            

            var i = 0;

            foreach (var group in OutputListGroupOrdered)
            {
                List<OutputList> SubList = new List<OutputList>();
                
                foreach (var list in group)
                { 
                    SubList.Add(new OutputList { ID = list.ID, Group = list.Group, Field = list.Field, Value = list.Value });
                }

                List.Add(new OutputListGroup { ID = i, Group = group.Key, Output = SubList});

                i = i + 1;
            }

            return List;
        }

        public void CalculateAction(List<CategoryViewModel> jCategory)
        {
   

            foreach (var group in jCategory)
            {
                foreach (var item in group.Functions)
                {
                    if (item.Function == "Input")
                    {
                        if(item.Type == "Date")
                        {
                            if(item.Output != "" && item.Output != "0")
                            {
                                DateTime datestring = Convert.ToDateTime(item.Output);
                                var shortdatestring = datestring.ToShortDateString();
                                item.Output = Convert.ToString(shortdatestring);
                            }
                            else
                            {
                                item.Output = "";
                            }

                        }
                        else if (item.Type == "Decimal")
                        {
                            if(item.Output == "" || item.Output == null)
                            {
                                item.Output = "0";
                            }
                        }

                        OutputList.Add(new OutputList { ID = Convert.ToString(item.ID), Field = item.Name, Value = item.Output, Group = group.Name });
                    }
                    else
                    {
                        if (item.Parameter.Count > 0)
                        {
                            string logic = null;
                            bool logicparse = true;
                            string MathString = null;
                            bool PowOpen = false;

                            foreach (var bit in item.Logic)
                            {

                                dynamic InputA = Config.VariableReplace(jCategory, bit.Input1, group.ID, item.ID);
                                dynamic InputB = Config.VariableReplace(jCategory, bit.Input2, group.ID, item.ID);

                                
                                string Logic = null;
                                if(bit.LogicInd == "NotEqual")
                                {
                                    Logic = "<>";
                                }
                                else if(bit.LogicInd == "Greater")
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

                                if (InputADeciSucceeded == true && InputBDeciSucceeded == true)
                                {
                                    logic = "if(" + InputADeci + Logic + InputBDeci + ",true,false)";
                                }
                                else if(InputADeciSucceeded == true && InputBDeciSucceeded == false)
                                {
                                    logic = "if(" + InputADeci + Logic + "'" + InputBDeci + "'" + ",true,false)";
                                }
                                else if (InputADeciSucceeded == false && InputBDeciSucceeded == true)
                                {
                                    logic = "if(" + "'" + InputADeci + "'" + Logic + InputBDeci + ",true,false)";
                                }
                                else
                                {
                                    string inputA = Convert.ToString(InputA);
                                    string inputB = Convert.ToString(InputB);
                                    logic = "if(" + "'" + inputA + "'" + Logic + "'" + inputB + "'" + ",true,false)";
                                }


                                Expression ex = new Expression(logic);
                                logicparse = Convert.ToBoolean(ex.Evaluate());
                            }
                            if (logicparse == true)
                            {
                                int paramCount = 1;
                                foreach (var param in item.Parameter)
                                {
                                    string jparameters = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                                    if (item.Function == "Maths")
                                    {
                                        string formula = null;
                                        Maths Maths = new Maths();
                                        Maths parameters = (Maths)javaScriptSerializ­er.Deserialize(jparameters, typeof(Maths));
                                        
                                        dynamic InputA = Config.VariableReplace(jCategory, parameters.Input1, group.ID, item.ID);
                                        dynamic InputB = Config.VariableReplace(jCategory, parameters.Input2, group.ID, item.ID);
                                        string Bracket1 = Convert.ToString(parameters.Bracket1);
                                        string Input1 = Convert.ToString(InputA);
                                        string Logic = Convert.ToString(parameters.Logic);
                                        string Input2 = Convert.ToString(InputB);
                                        string Bracket2 = Convert.ToString(parameters.Bracket2);
                                        string Rounding = Convert.ToString(parameters.Rounding);
                                        string RoundingType = Convert.ToString(parameters.RoundingType);
                                        string Logic2 = Convert.ToString(parameters.Logic2);
                                        if (Rounding == "")
                                        {
                                            Rounding = "2";
                                        }

                                        if (Logic == "Pow")
                                        {
                                            formula = Logic + '(' + Input1 + ',' + Input2 + ')';
                                        }
                                        else
                                        {
                                            formula = Input1 + Logic + Input2;
                                        }

                                        string MathString1;
                                        

                                        if (Logic2 == "Pow")
                                        {
                                            MathString1 = string.Concat(Logic2,"(",MathString, Bracket1, formula, Bracket2, ",");
                                            PowOpen = true;
                                        }
                                        else
                                        {
                                            MathString1 = string.Concat(MathString, Bracket1, formula, Bracket2, Logic2);
                                        }

                                        if(Logic2 != "Pow" && PowOpen == true)
                                        {
                                            MathString1 = string.Concat(MathString1, ")");
                                            PowOpen = false;
                                        }
                                        
                                        MathString = MathString1;
                                        
                                        if(paramCount == item.Parameter.Count)
                                        {

                                            Expression e = new Expression(MathString);
                                            var Calculation = e.Evaluate();

                                            decimal Output = Convert.ToDecimal(Calculation);

                                            if(RoundingType == "up")
                                            {

                                               Output = Math.Round(Math.Ceiling(Output * 100) / 100, Convert.ToInt16(Rounding));

                                            }
                                            else if(RoundingType == "down")
                                            {

                                                if(Convert.ToInt16(Rounding) == 0)
                                                {
                                                    Output = Math.Truncate(Output);
                                                }
                                                else
                                                {
                                                    Output = Math.Round(Math.Floor(Output * 100) / 100, Convert.ToInt16(Rounding));
                                                }
                                                

                                            }
                                            else
                                            {
                                                Output = Math.Round(Output, Convert.ToInt16(Rounding));
                                            }

                                            item.Output = Convert.ToString(Output);
                                        }

                                        paramCount = paramCount + 1;
                                        InputA = null;
                                        InputB = null;

                                    }
                                    else if (item.Function == "Period")
                                    {

                                        DateFunctions DateFunctions = new DateFunctions();
                                        Period Dates = new Period();
                                        Period parameters = (Period)javaScriptSerializ­er.Deserialize(jparameters, typeof(Period));
                                        dynamic InputA = Config.VariableReplace(jCategory, parameters.Date1, group.ID, item.ID);
                                        dynamic InputB = Config.VariableReplace(jCategory, parameters.Date2, group.ID, item.ID);

                                        if(InputA != "" && InputB != "" && InputA != "01/01/0001" && InputB != "01/01/0001")
                                        {
                                            System.DateTime Date1 = DateTime.Parse(InputA);
                                            System.DateTime Date2 = DateTime.Parse(InputB);
                                            String DateAdjustmentType = parameters.DateAdjustmentType;
                                            Boolean Inclusive = parameters.Inclusive;
                                            Double DaysinYear = parameters.DaysinYear;

                                            if (Date1 <= Date2)
                                            {

                                                if (DateAdjustmentType == "YearsDays")
                                                {
                                                    item.Output = Convert.ToString(DateFunctions.YearsDaysBetween(Date1, Date2, Inclusive, DaysinYear));
                                                }
                                                else if (DateAdjustmentType == "YearsMonths")
                                                {
                                                    item.Output = Convert.ToString(DateFunctions.YearsMonthsBetween(Date1, Date2, Inclusive, DaysinYear));
                                                }

                                                else if (DateAdjustmentType == "Years")
                                                {
                                                    item.Output = Convert.ToString(DateFunctions.YearsBetween(Date1, Date2, Inclusive, DaysinYear));
                                                }

                                                else if (DateAdjustmentType == "Months")
                                                {
                                                    item.Output = Convert.ToString(DateFunctions.GetMonthsBetween(Date1, Date2, false));
                                                }

                                                else if (DateAdjustmentType == "Days")
                                                {
                                                    item.Output = Convert.ToString(DateFunctions.DaysBetween(Date1, Date2, Inclusive, DaysinYear));
                                                }

                                                InputA = null;
                                                InputB = null;
                                            }
                                            else
                                            {
                                                item.Output = Convert.ToString(0);
                                            }
                                        }
                                        else
                                        {
                                            item.Output = Convert.ToString(0);
                                        }


 
                                    }
                                    else if (item.Function == "Factors")
                                    {
                                        LookupFunctions FactorFunctions = new LookupFunctions();
                                        Factors parameters = (Factors)javaScriptSerializ­er.Deserialize(jparameters, typeof(Factors));
                                        dynamic InputA = Config.VariableReplace(jCategory, parameters.LookupValue, group.ID, item.ID);
                                        dynamic InputB = Config.VariableReplace(jCategory, parameters.RowMatchValue, group.ID, item.ID);
                                        if (parameters.RowMatch == true)
                                        {
                                           parameters.ColumnNo =  FactorFunctions.CSVColumnNumber(parameters.TableName, parameters.RowMatchRowNo, InputB);
                                        }

                                        if(parameters.ColumnNo > 0 )
                                        {
                                            if (parameters.LookupType == "Date")
                                            {
                                                DateTime LookupValue;
                                                DateTime.TryParse(InputA, out LookupValue);
                                                item.Output = Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 1, parameters.ColumnNo));
                                            }
                                            else if (parameters.LookupType == "Decimal")
                                            {
                                                decimal LookupValue;
                                                decimal.TryParse(InputA, out LookupValue);

                                                item.Output = Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 1, parameters.ColumnNo));
                                            }
                                            else
                                            {
                                                string LookupValue;
                                                LookupValue = InputA;
                                                item.Output = Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, Convert.ToString(LookupValue), 2, parameters.ColumnNo));
                                            }

                                            item.Type = parameters.OutputType;

                                        }
                                        else
                                        {
                                            item.Type = parameters.OutputType;
                                            if(parameters.OutputType == "Decimal")
                                            {
                                                item.Output = "0";
                                            }
                                            else
                                            {
                                                item.Output = "";
                                            }
                                           
                                        }

                                    }
                                    else if (item.Function == "Dates")
                                    {
                                        DateFunctions DatesFunctions = new DateFunctions();
                                        Dates parameters = (Dates)javaScriptSerializ­er.Deserialize(jparameters, typeof(Dates));
                                        dynamic InputA = Config.VariableReplace(jCategory, parameters.Date1, group.ID, item.ID);
                                        dynamic InputB = Config.VariableReplace(jCategory, parameters.Date2, group.ID, item.ID);
                                        dynamic InputC = Config.VariableReplace(jCategory, parameters.Period, group.ID, item.ID);

                                        DateTime Date1;
                                        DateTime Date2;
                                        Decimal Period;

                                        DateTime.TryParse(InputA, out Date1);
                                        DateTime.TryParse(InputB, out Date2);
                                        Decimal.TryParse(InputC, out Period);

                                        DateTime date = DatesFunctions.DateAdjustment(parameters.Type, Convert.ToString(Date1), Convert.ToString(Date2), parameters.PeriodType, Period, parameters.Adjustment, parameters.Day, parameters.Month);

                                        DateTime datestring = date.Date;

                                        var shortdatestring = datestring.ToShortDateString();

                                        item.Output = Convert.ToString(shortdatestring);

                                        InputA = null;
                                        InputB = null;
                                        InputC = null;
                                    }
                                    else if (item.Function == "DatePart")
                                    {
                                        DateFunctions DatesFunctions = new DateFunctions();
                                        DatePart parameters = (DatePart)javaScriptSerializ­er.Deserialize(jparameters, typeof(DatePart));
                                        dynamic InputA = Config.VariableReplace(jCategory, parameters.Date1, group.ID, item.ID);
    
                                        DateTime Date1;
                                        DateTime.TryParse(InputA, out Date1);

                                        int DatePart = DatesFunctions.GetDatePart(parameters.Part, Date1);

                                        item.Output = Convert.ToString(DatePart);

                                        InputA = null;

                                    }

                                    else if (item.Function == "MathsFunctions")
                                    {
                                        MathFunctions MathFunctions = new MathFunctions();
                                        MathsFunctions parameters = (MathsFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(MathsFunctions));
                                        dynamic InputA = Config.VariableReplace(jCategory, parameters.Number1, group.ID, item.ID);
                                        dynamic InputB = Config.VariableReplace(jCategory, parameters.Number2, group.ID, item.ID);

                                        decimal InputADeci;
                                        decimal InputBDeci;

                                        decimal.TryParse(InputA, out InputADeci);
                                        decimal.TryParse(InputB, out InputBDeci);
                                        Decimal Output;

                                        Output = 0;

                                        if(parameters.Type == "Abs")
                                        {
                                            Output = MathFunctions.Abs(InputADeci);
                                        }
                                        else if(parameters.Type == "Ceiling")
                                        {
                                            Output = MathFunctions.Ceiling(InputADeci);
                                        }
                                        else if (parameters.Type == "Floor")
                                        {
                                            Output = MathFunctions.Floor(InputADeci);
                                        }
                                        else if (parameters.Type == "Max")
                                        {
                                            Output = MathFunctions.Max(InputADeci, InputBDeci);
                                        }
                                        else if (parameters.Type == "Min")
                                        {
                                            Output = MathFunctions.Min(InputADeci, InputBDeci);
                                        }
                                        else if (parameters.Type == "Truncate")
                                        {
                                            Output = MathFunctions.Truncate(InputADeci);
                                        }

                                        item.Output = Convert.ToString(Output);

                                        InputA = null;
                                        InputB = null;

                                    }
                                }

                                if (item.ExpectedResult == null || item.ExpectedResult == "")
                                {
                                    item.Pass = "Nil";

                                }
                                else if (item.ExpectedResult == item.Output)
                                {
                                    item.Pass = "true";
                                }
                                else
                                {
                                    item.Pass = "false";
                                }

                                OutputList.Add(new OutputList { ID = Convert.ToString(item.ID), Field = item.Name, Value = item.Output, Group = group.Name });

                            }
                            else
                            {
                                dynamic LogicReplace = Config.VariableReplace(jCategory, item.Name, group.ID, item.ID);

                                if(Convert.ToString(LogicReplace) == Convert.ToString(item.Name))
                                {
                                    item.Output = null;
                                }
                                else
                                {
                                    item.Output = Convert.ToString(LogicReplace);
                                }
                                
                                item.Pass = "miss";
                            }
                        }
                    }
                }
            }
        }


    }
}