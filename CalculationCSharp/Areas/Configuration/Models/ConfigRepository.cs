using CalculationCSharp.Areas.Configuration.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;


namespace CalculationCSharp.Areas.Configuration.Models
{
    public class ConfigRepository
    {
        CategoryViewModel Configuration = new CategoryViewModel();
        ConfigViewModel Functions = new ConfigViewModel();

        public List<CategoryViewModel> GetConfig(CategoryViewModel Config)
        {
            var Configuration = new List<CategoryViewModel>();
            if (HttpContext.Current.Cache["config"] == null)
            {
                if (Config == null)
                {
                    Configuration.Add(new CategoryViewModel { ID = 1, Name = "Service", Functions = new List<ConfigViewModel>() });
                    HttpContext.Current.Cache["config"] = Configuration;
                }
                else
                {

                }
            }

            return (List<CategoryViewModel>)HttpContext.Current.Cache["config"];
        }

        public void EditConfig(JObject Data)
        {
            
            dynamic json = Data;

            json = json.data;

            Functions.ID = json.ID;
            Functions.Name = json.Name;
            Functions.Function = json.Function;
            Functions.Category = json.Category;
            Functions.Type = json.Type;
            Functions.Output = json.Output;

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = Convert.ToString(json.Parameter);

            List<dynamic> jMaths = (List<dynamic>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<dynamic>));

            Functions.Parameter = jMaths;

        }

        public void UpdateConfig(List<CategoryViewModel> Config)
        {
            HttpContext.Current.Cache["config"] = Config;
        }


    }




  
}