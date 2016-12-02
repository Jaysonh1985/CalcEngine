// Copyright (c) 2016 Project AIM
using System;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using NCalc;
using CalculationCSharp.Models.Calculation;
using System.Linq;
using log4net;
using System.Web;
using CalculationCSharp.Models;

namespace CalculationCSharp.Areas.Configuration.Models.Actions
{
    public class Calculate
    {
        public List<OutputList> OutputList = new List<OutputList>();
        public JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        public List<CategoryViewModel> jCategory = new List<CategoryViewModel>();
        CalculationCSharp.Areas.Configuration.Models.ConfigFunctions Config = new CalculationCSharp.Areas.Configuration.Models.ConfigFunctions();
        public Input InputFunctions = new Input();
        private CalculationDBContext db = new CalculationDBContext();
        private static readonly ILog logger = LogManager.GetLogger(typeof(Calculate));
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
                    //int index = SubList.FindIndex(a => a.Field == list.Field);
                    //if (index == -1)
                    //{
                    //    SubList.Add(new OutputList { ID = list.ID, Group = list.Group, Field = list.Field, Value = list.Value });
                    //}
                    //else
                    //{
                    //    SubList.RemoveAt(index);
                    //    SubList.Add(new OutputList { ID = list.ID, Group = list.Group, Field = list.Field, Value = list.Value });
                    //}

                }              

                List.Add(new OutputListGroup { ID = i, Group = group.Key, Output = SubList});

                i = i + 1;
            }

            return List;
        }
        //Calcuation Controller Action
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
                        //Logic check at Column Level
                        string colLogic = null;
                        bool colLogicParse = true;
                        if(group.Logic != null)
                        {
                            foreach (var bit in group.Logic)
                            {
                                Logic Logic = new Logic();
                                colLogic = Logic.Output(jCategory, bit, group.ID, 0);
                                Expression ex = new Expression(colLogic);
                                try
                                {
                                    colLogicParse = Convert.ToBoolean(ex.Evaluate());
                                }
                                catch (Exception exception)
                                {
                                    logger.Error(exception);
                                }                               
                            }
                        }
                        if (item.Parameter.Count > 0)
                        {
                            string logic = null;
                            bool logicparse = true;
                            string MathString = null;
                            bool PowOpen = false;
                            //Logic check at column level
                            if (colLogicParse == true)
                            {
                              foreach (var bit in item.Logic)
                                {
                                    Logic Logic = new Logic();
                                    logic = Logic.Output(jCategory, bit, group.ID, item.ID);
                                    Expression ex = new Expression(logic);

                                    try
                                    {
                                        logicparse = Convert.ToBoolean(ex.Evaluate());
                                    }
                                    catch (Exception exception)
                                    {
                                        logger.Error(exception);
                                    }
                                                       
                                }
                            }
                            else
                            {
                                logicparse = false;
                            }
                            //Run code if logic if met at column and row level
                            if (logicparse == true)
                            {
                                int paramCount = 1;
                                foreach (var param in item.Parameter)
                                {
                                    string jparameters = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                                    logger.Debug("Column Name(" + group.ID + ") - " + group.Name + " || Row Name(" + item.ID +") - " + item.Name);
                                    if (item.Function == "Maths")
                                    {
                                        Maths Maths = new Maths();
                                        Maths parameters = (Maths)javaScriptSerializ­er.Deserialize(jparameters, typeof(Maths));
                                        MathString = Maths.Output(jparameters,jCategory,group.ID,item.ID,MathString,PowOpen);
                                        PowOpen = Maths.PowOpen(jparameters, PowOpen);
                                        if (paramCount == item.Parameter.Count)
                                        {
                                            Expression e = new Expression(MathString);
                                            var Calculation = e.Evaluate();
                                            bool DeciParse;
                                            decimal CalculationDeci;
                                            string Rounding;
                                            DeciParse = decimal.TryParse(Convert.ToString(Calculation), out CalculationDeci);
                                            Rounding = Convert.ToString(parameters.Rounding);

                                            if (Rounding == null || Rounding == "")
                                            {
                                                Rounding = "2";
                                            }
                                            if (DeciParse == true)
                                            {
                                                decimal Output = CalculationDeci;
                                                MathematicalFunctions MathematicalFunctions = new MathematicalFunctions();

                                                try
                                                {
                                                    Output = MathematicalFunctions.Rounding(Convert.ToString(parameters.RoundingType), Rounding, Output);
                                                }
                                                catch (Exception ex)
                                                {
                                                    logger.Error(ex);
                                                }
                                               
                                                item.Output = Convert.ToString(Output);
                                            }
                                            else
                                            {
                                                item.Output = "0";
                                            }                                                      
                                        }
                                        paramCount = paramCount + 1;
                                    }
                                    else if (item.Function == "ErrorsWarnings")
                                    {
                                        ErrorsWarnings Errors = new ErrorsWarnings();
                                        ErrorsWarnings parameters = (ErrorsWarnings)javaScriptSerializ­er.Deserialize(jparameters, typeof(ErrorsWarnings));
                                        item.Name = parameters.Type;
                                        item.Output = parameters.String1;
                                    }
                                    else if (item.Function == "Comments")
                                    {
                                        Comments Errors = new Comments();
                                        Comments parameters = (Comments)javaScriptSerializ­er.Deserialize(jparameters, typeof(Comments));
                                        item.Output = parameters.String1;
                                    }
                                    else if (item.Function == "Period")
                                    {
                                        DateFunctions DateFunctions = new DateFunctions();
                                        Period Periods = new Period();
                                        try
                                        {
                                            item.Output = Periods.Output(jparameters, jCategory, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                        }                                   
                                    }
                                    else if (item.Function == "Factors")
                                    {
                                        Factors Factors = new Factors();
                                        Factors parameters = (Factors)javaScriptSerializ­er.Deserialize(jparameters, typeof(Factors));
                                        try
                                        {
                                            item.Output = Factors.Output(jparameters, jCategory, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);                                           
                                        }
                                       
                                        item.Type = parameters.OutputType;
                                    }
                                    else if (item.Function == "DateAdjustment")
                                    {
                                        Dates Dates = new Dates();
                                        try
                                        {
                                            item.Output = Dates.Output(jparameters, jCategory, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                        }
                                    }
                                    else if (item.Function == "DatePart")
                                    {
                                        DatePart DateParts = new DatePart();
                                        try
                                        {
                                            item.Output = DateParts.Output(jparameters, jCategory, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                        }                                       
                                    }

                                    else if (item.Function == "MathsFunctions")
                                    {
                                        MathsFunctions MathsFunctions = new MathsFunctions();
                                        try
                                        {
                                            item.Output = MathsFunctions.Output(jparameters, jCategory, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                        }                                       
                                    }
                                    else if (item.Function == "ArrayFunctions")
                                    {
                                        ArrayFunctions ArrayFunctions = new ArrayFunctions();
                                        ArrayFunctions parameters = (ArrayFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(ArrayFunctions));
                                        try
                                        {
                                            item.Output = ArrayFunctions.Output(jparameters, jCategory, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                        }
                                        

                                        if(parameters.Function == "Count")
                                        {
                                            item.Type = "Decimal";
                                        }
                                        else
                                        {
                                            item.Type = parameters.LookupType;
                                        }
                                        
                                    }
                                    else if (item.Function == "StringFunctions")
                                    {
                                        StringFunctions StringFunctions = new StringFunctions();
                                        StringFunctions  parameters = (StringFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(StringFunctions));
                                        try
                                        {
                                            item.Output = StringFunctions.Output(jparameters, jCategory, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                        }
                                        
                                        if(parameters.Type == "Len")
                                        {
                                            item.Type = "Decimal";
                                        }
                                        else
                                        {
                                            item.Type = "String";
                                        }
                                    }
                                }
                                //Expected results on the builder this sets the required ones
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
                                //Ignores the row if logic is not met
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

                                OutputList.Add(new OutputList { ID = Convert.ToString(item.ID), Field = item.Name, Value = item.Output, Group = group.Name });
                            }
                        }
                    }
                }
            }
        }
    }
}