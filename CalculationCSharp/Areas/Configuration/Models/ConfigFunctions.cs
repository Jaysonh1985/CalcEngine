using System.Collections.Generic;
using System.Linq;


namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ConfigFunctions
    {

        public dynamic VariableReplace(List<CategoryViewModel> CategoryViewModel, string Input, int ID)
        {
            dynamic element = null;
            dynamic Output = null;

            foreach (var item in CategoryViewModel)
            {
                List<ConfigViewModel> ConfigViewModel = new List<ConfigViewModel>();

                ConfigViewModel = item.Functions;

                element = ConfigViewModel.Where(a => a.Name == Input).LastOrDefault();

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