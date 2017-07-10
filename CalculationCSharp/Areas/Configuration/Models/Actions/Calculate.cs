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
using System.ComponentModel.DataAnnotations;
using CalculationCSharp.Models.ArrayFunctions;

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
                    SubList.Add(new OutputList { ID = list.ID, Group = list.Group, Field = list.Field, Value = list.Value, SubOutput = list.SubOutput, Description = list.Description, Function = list.Function, Parameter = list.Parameter });
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
                        OutputList.Add(new OutputList { ID = Convert.ToString(item.ID), Field = item.Name, Value = item.Output, Group = group.Name, Description = item.Description, Function = item.Function, Parameter = item.Parameter });
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
                                var grouplastLogic = group.Logic.Last();
                                string grouplastLogicOperator = grouplastLogic.Operator;
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
                                    throw new HttpException(exception.ToString());
                                }

                                if (grouplastLogicOperator == "AND" && colLogicParse == false)
                                {
                                    break;
                                }
                                else if (grouplastLogicOperator == "OR" && colLogicParse == true)
                                {
                                    colLogicParse = true;
                                    break;
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
                                    var lastLogic = item.Logic.Last();
                                    string lastLogicOperator = lastLogic.Operator;
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
                                        throw new HttpException(exception.ToString());
                                    }

                                    if (lastLogicOperator == "AND" && logicparse == false)
                                    {
                                        break;
                                    }
                                    else if (lastLogicOperator == "OR" && logicparse == true)
                                    {
                                        logicparse = true;
                                        break;
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
                                        MathString = Maths.Output(jparameters, jCategory, group.ID, item.ID, MathString, PowOpen);
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
                                                    throw new HttpException(ex.ToString());
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
                                            throw new HttpException(ex.ToString());
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
                                            throw new HttpException(ex.ToString());
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
                                            throw new HttpException(ex.ToString());
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
                                            throw new HttpException(ex.ToString());
                                        }
                                    }
                                    else if (item.Function == "Return")
                                    {
                                        Return Return = new Return();
                                        try
                                        {
                                            int returnCounter = 0;
                                            foreach(var returnParam in item.Parameter.ToList())
                                            {
                                                string jparameters1 = Newtonsoft.Json.JsonConvert.SerializeObject(returnParam);
                                                Return parameters = (Return)javaScriptSerializ­er.Deserialize(jparameters1, typeof(Return));
                                                parameters.Output = Convert.ToString(Return.Output1(jparameters1, jCategory, group.ID, item.ID));
                                                string jparameters2 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);
                                                item.Parameter[returnCounter] = javaScriptSerializer.DeserializeObject(jparameters2);
                                                
                                                returnCounter ++;
                                            }
                                            return;
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                            throw new HttpException(ex.ToString());
                                        }
                                    }
                                    else if (item.Function == "MathsFunctions")
                                    {
                                        MathsFunctions MathsFunctions = new MathsFunctions();
                                        try
                                        {
                                            item.Output = MathsFunctions.Output(jparameters, jCategory, group.ID, item.ID, group);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                            throw new HttpException(ex.ToString());
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
                                            throw new HttpException(ex.ToString());
                                        }


                                        if (parameters.Function == "Count")
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
                                        StringFunctions parameters = (StringFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(StringFunctions));
                                        try
                                        {
                                            item.Output = StringFunctions.Output(jparameters, jCategory, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                            throw new HttpException(ex.ToString());
                                        }

                                        if (parameters.Type == "Len")
                                        {
                                            item.Type = "Decimal";
                                        }
                                        else
                                        {
                                            item.Type = "String";
                                        }
                                    }
                                    else if (item.Function == "LogicFunctions")
                                    {
                                        LogicFunctions LogicFunctions = new LogicFunctions();
                                        LogicFunctions parameters = (LogicFunctions)javaScriptSerializ­er.Deserialize(jparameters, typeof(LogicFunctions));
                                        try
                                        {
                                            item.Output = LogicFunctions.Output(jCategory, parameters, group.ID, item.ID);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                            throw new HttpException(ex.ToString());
                                        }
                                    }
                                    else if (item.Function == "Table")
                                    {
                                        Table LogicFunctions = new Table();
                                        Table parameters = (Table)javaScriptSerializ­er.Deserialize(jparameters, typeof(Table));
                                        try
                                        {
                                            ArrayBuildingFunctions ArrayBuilder = new ArrayBuildingFunctions();
                                            string[] Numbers1parts = null;
                                            //Returns array
                                            Numbers1parts = ArrayBuilder.InputArrayBuilder(parameters.Variable, jCategory, group.ID, item.ID);
                                            parameters.Result = string.Join("~", Numbers1parts);
                                            param["Result"] = parameters.Result;
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                            throw new HttpException(ex.ToString());
                                        }
                                    }
                                    else if (item.Function == "Function")
                                    {
                                        Function Functions = new Function();
                                        Function parameters = (Function)javaScriptSerializ­er.Deserialize(jparameters, typeof(Function));
                                        FunctionConfiguration calcFunction = db.FunctionConfiguration.Find(Convert.ToInt32(parameters.ID));
                                        try
                                        {
                                            List<CategoryViewModel> calcFunctionConfig = (List<CategoryViewModel>)javaScriptSerializ­er.Deserialize(calcFunction.Configuration, typeof(List<CategoryViewModel>));
                                            //replace inputs from main configuration to function
                                            foreach (var row in calcFunctionConfig[0].Functions)
                                            {
                                                int index = parameters.Input[0].Functions.FindIndex(a => a.Name == row.Name);
                                                if (index >= 0)
                                                {
                                                    row.Output = parameters.Input[0].Functions[index].Output;
                                                }
                                                else
                                                {
                                                    row.Output = null;
                                                }
                                            }

                                            foreach (var input in calcFunctionConfig[0].Functions)
                                            {
                                                if (input.Output != null)
                                                {
                                                    input.Output = Convert.ToString(Functions.Output(jparameters, jCategory, group.ID, item.ID, input.Output, input.Type));
                                                }
                                            }
                                            //run the function with the new inputs
                                            Calculate Calculate = new Calculate();
                                            calcFunctionConfig = Calculate.DebugResults(calcFunctionConfig);
                                            item.SubOutput = Calculate.OutputResults(calcFunctionConfig);
                                            foreach (var col in calcFunctionConfig)
                                            {
                                                int index = col.Functions.FindIndex(a => a.Function == "Return");
                                                if (index >= 0)
                                                {
                                                    item.Output = col.Functions[index].Output;
                                                    List<Return> returnList = new List<Return>();
                                                    foreach (var thing in col.Functions[index].Parameter)
                                                    {
                                                        Return returnObj = new Actions.Return();
                                                        returnObj.Type = thing["Type"];
                                                        returnObj.Name = thing["Name"];
                                                        returnObj.Output = thing["Output"];
                                                        returnObj.Variable = thing["Variable"];
                                                        returnList.Add(returnObj);                                                       
                                                    };
                                                    item.FunctionOutput = returnList;
                                                };
                                            };
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex);
                                            logger.Error("Missing Function, please check this is available");
                                            throw new HttpException("Missing Function, please check this is available");
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
                                OutputList.Add(new OutputList { ID = Convert.ToString(item.ID), Field = item.Name, Value = item.Output, Group = group.Name, SubOutput = item.SubOutput, Description = item.Description, Function = item.Function, Parameter = item.Parameter });
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

                                OutputList.Add(new OutputList { ID = Convert.ToString(item.ID), Field = item.Name, Value = item.Output, Group = group.Name, SubOutput = item.SubOutput, Description = item.Description, Function = item.Function, Parameter = item.Parameter });
                            }
                        }
                    }
                }
            }
        }
    }
}