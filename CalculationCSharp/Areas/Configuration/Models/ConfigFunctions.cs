// Copyright (c) 2016 Project AIM
using System.Collections.Generic;
using System.Linq;


namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ConfigFunctions
    {

        public dynamic VariableReplace(List<CategoryViewModel> CategoryViewModel, string Input,int colID, int rowID)
        {
            dynamic element = null;
            dynamic Output = null;
            dynamic elementType = null;
            foreach (var item in CategoryViewModel)
            {
                List<ConfigViewModel> ConfigViewModel = new List<ConfigViewModel>();

                ConfigViewModel = item.Functions;

                if(item.ID < colID)
                {
                    element = ConfigViewModel.Where(a => a.Name == Input).LastOrDefault();
                }
                else if (item.ID == colID)
                {
                    element = ConfigViewModel.Where(a => a.Name == Input && a.ID < rowID).LastOrDefault();
                }
                else
                {
                    element = null;
                }

                if(element != null)
                {
                    elementType = element;
                    Output = element.Output; 
                }

            }

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

        public dynamic CategoryVariableReplace(List<CategoryViewModel> CategoryViewModel, string Input, int colID, int rowID)
        {
            dynamic element = null;
            dynamic Output = null;

            foreach (var item in CategoryViewModel)
            {

                if (item.ID < colID)
                {
                    element = CategoryViewModel.Where(a => a.Name == Input).LastOrDefault();
                }
                else if (item.ID == colID)
                {
                    element = CategoryViewModel.Where(a => a.Name == Input && a.ID < rowID).LastOrDefault();
                }
                else
                {
                    element = null;
                }

                if (element != null)
                {
                    Output = element.Output;
                }

            }

            if (Output == null)
            {
                if (element != null)
                {
                    if (element.Type == "Decimal")
                    {
                        return 0;
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