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
                    Output = element.Output; 
                }

            }

            if (Output == null)
            {
                return Input;
            }
            else
            {
                return Output;
            }

        }

    }
}