using System;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using NCalc;
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
        public Input InputFunctions = new Input();
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
                    int index = SubList.FindIndex(a => a.Field == list.Field);
                    if (index == -1)
                    {
                        SubList.Add(new OutputList { ID = list.ID, Group = list.Group, Field = list.Field, Value = list.Value });
                    }
                    else
                    {
                        SubList.RemoveAt(index);
                        SubList.Add(new OutputList { ID = list.ID, Group = list.Group, Field = list.Field, Value = list.Value });
                    }

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
                        item.Output = InputFunctions.Output(item.Type, item.Output);
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
                                Logic Logic = new Logic();                            
                                logic = Logic.Output(jCategory, bit, group.ID, item.ID);
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
                                            MathString1 = string.Concat(Logic2, "(", MathString, Bracket1, formula, Bracket2, ",");
                                            PowOpen = true;
                                        }
                                        else
                                        {
                                            MathString1 = string.Concat(MathString, Bracket1, formula, Bracket2, Logic2);
                                        }

                                        if (Logic2 != "Pow" && PowOpen == true)
                                        {
                                            MathString1 = string.Concat(MathString1, ")");
                                            PowOpen = false;
                                        }

                                        MathString = MathString1;

                                        if (paramCount == item.Parameter.Count)
                                        {
                                            Expression e = new Expression(MathString);
                                            var Calculation = e.Evaluate();
                                            decimal Output = Convert.ToDecimal(Calculation);

                                            if (RoundingType == "up")
                                            {
                                                Output = Math.Round(Math.Ceiling(Output * 100) / 100, Convert.ToInt16(Rounding));
                                            }
                                            else if (RoundingType == "down")
                                            {
                                                if (Convert.ToInt16(Rounding) == 0)
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
                                        Period Periods = new Period();
                                        item.Output = Periods.Output(jparameters, jCategory, group.ID, item.ID);
                                    }
                                    else if (item.Function == "Factors")
                                    {
                                        Factors Factors = new Factors();
                                        Factors parameters = (Factors)javaScriptSerializ­er.Deserialize(jparameters, typeof(Factors));
                                        item.Output = Factors.Output(jparameters, jCategory, group.ID, item.ID, item.Type);
                                        item.Type = parameters.OutputType;
                                    }
                                    else if (item.Function == "Dates")
                                    {
                                        Dates Dates = new Dates();
                                        item.Output = Dates.Output(jparameters, jCategory, group.ID, item.ID);
                                    }
                                    else if (item.Function == "DatePart")
                                    {
                                        DatePart DateParts = new DatePart();
                                        item.Output = DateParts.Output(jparameters, jCategory, group.ID, item.ID);
                                    }

                                    else if (item.Function == "MathsFunctions")
                                    {
                                        MathsFunctions MathsFunctions = new MathsFunctions();
                                        item.Output = MathsFunctions.Output(jparameters, jCategory, group.ID, item.ID);
                                    }
                                    else if (item.Function == "ArrayFunctions")
                                    {
                                        ArrayFunctions ArrayFunctions = new ArrayFunctions();
                                        ArrayFunctions parameters = (ArrayFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(ArrayFunctions));
                                        item.Output = ArrayFunctions.Output(jparameters, jCategory, group.ID, item.ID);
                                        item.Type = parameters.LookupType;
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