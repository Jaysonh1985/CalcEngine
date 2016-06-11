using System.Collections.Generic;
using System.Linq;


namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ConfigFunctions
    {

        public dynamic VariableReplace(List<CategoryViewModel> CategoryViewModel, string Input, int ID)
        {
           dynamic element = null;

            foreach (var item in CategoryViewModel)
            {
                List<ConfigViewModel> ConfigViewModel = new List<ConfigViewModel>();

                ConfigViewModel = item.Functions;

                element = ConfigViewModel.Where(a => a.Name == Input && a.ID <= ID).LastOrDefault();

            }

            if (element == null)
            {
                return Input;
            }
            else
            {
                return element.Output;
            }

        }

    }
}