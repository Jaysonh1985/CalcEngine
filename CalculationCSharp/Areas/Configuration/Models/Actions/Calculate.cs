using CalculationCSharp.Areas.Configuration.Models;
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

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Calculate
    {
        public List<OutputList> OutputList = new List<OutputList>();
        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        public List<CategoryViewModel> jCategory = new List<CategoryViewModel>();

        public List<CategoryViewModel> DebugResults(List<CategoryViewModel> jCategory)
        {
            Calculate Calculate = new Calculate();
            Calculate.CalculateAction(jCategory);
            return jCategory;
        }
        public List<OutputList> OutputResults(List<CategoryViewModel> jCategory)
        {
            Calculate Calculate = new Calculate();
            Calculate.CalculateAction(jCategory);
            return Calculate.OutputList;
        }

        public void CalculateAction(List<CategoryViewModel> jCategory)
        {
            foreach (var group in jCategory)
            {
                foreach (var item in group.Functions)
                {
                    if (item.Function == "Input")
                    {

                    }
                    else
                    {
                        if (item.Parameter.Count > 0)
                        {
                            string logic = null;
                            bool logicparse = true;

                            foreach (var bit in item.Logic)
                            {

                                string inputA = bit.Input1;
                                string Logic = bit.LogicInd;
                                string inputB = bit.Input2;

                                logic = "if(" + inputA + Logic + inputB + ",true,false)";
                                Expression ex = new Expression(logic);
                                logicparse = Convert.ToBoolean(ex.Evaluate());
                            }
                            if (logicparse == true)
                            {

                                foreach (var param in item.Parameter)
                                {
                                    string jparameters = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                                    if (item.Function == "Maths")
                                    {
                                        string formula = null;
                                        Maths Maths = new Maths();
                                        Maths parameters = (Maths)javaScriptSerializ­er.Deserialize(jparameters, typeof(Maths));
                                        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
                                        dynamic InputA = Config.VariableReplace(jCategory, parameters.Input1, item.ID);
                                        dynamic InputB = Config.VariableReplace(jCategory, parameters.Input2, item.ID);
                                        string Input1 = Convert.ToString(InputA);
                                        string Logic = Convert.ToString(parameters.Logic);
                                        string Input2 = Convert.ToString(InputB);
                                        string Rounding = Convert.ToString(parameters.Rounding);

                                        if (Rounding == "0")
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

                                        //Apply rounding
                                        formula = "Round(" + formula + "," + Rounding + ")";
                                        Expression e = new Expression(formula);
                                        var Calculation = e.Evaluate();
                                        item.Output = Convert.ToString(Convert.ToDecimal(Calculation));
                                    }
                                    else if (item.Function == "Period")
                                    {
                                        DateFunctions DateFunctions = new DateFunctions();
                                        Period Dates = new Period();
                                        Period parameters = (Period)javaScriptSerializ­er.Deserialize(jparameters, typeof(Period));
                                        System.DateTime Date1 = DateTime.Parse(parameters.Date1);
                                        System.DateTime Date2 = DateTime.Parse(parameters.Date2);
                                        String DateAdjustmentType = parameters.DateAdjustmentType;
                                        Boolean Inclusive = parameters.Inclusive;
                                        Double DaysinYear = parameters.DaysinYear;

                                        if (DateAdjustmentType == "YearsDays")
                                        {
                                            item.Output = Convert.ToString(DateFunctions.YearsDaysBetween(Date1, Date2, Inclusive, DaysinYear));
                                        }
                                        else if (DateAdjustmentType == "YearsMonths")
                                        {
                                            item.Output = Convert.ToString(DateFunctions.YearsMonthsBetween(Date1, Date2, Inclusive));
                                        }
                                    }
                                    else if (item.Function == "Factors")
                                    {
                                        LookupFunctions FactorFunctions = new LookupFunctions();
                                        Factors parameters = (Factors)javaScriptSerializ­er.Deserialize(jparameters, typeof(Factors));
                                        item.Output = Convert.ToString(FactorFunctions.CSVLookup(parameters.TableName, parameters.LookupValue, parameters.DataType, parameters.ColumnNo));
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
                                item.Pass = "miss";
                            }
                        }
                    }
                }
            }
        }


    }
}