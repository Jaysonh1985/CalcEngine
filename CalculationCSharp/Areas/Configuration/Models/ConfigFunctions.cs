using System.Collections.Generic;
using System.Linq;


namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ConfigFunctions
    {

        public dynamic VariableReplace(List<ConfigViewModel> ConfigViewModel, string Input, int ID)
        {

            var element = ConfigViewModel.Where(a => a.Name == Input && a.ID <= ID).LastOrDefault();

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