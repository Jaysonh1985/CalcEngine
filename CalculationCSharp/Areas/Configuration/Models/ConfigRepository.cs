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
        ConfigViewModel Configuration = new ConfigViewModel();

        public List<ConfigViewModel> GetConfig(ConfigViewModel Config)
        {
        
            if (HttpContext.Current.Cache["config"] == null)
            {
                if (Config == null)
                {
                    var Configuration = new List<ConfigViewModel>();

                    Configuration.Add(new ConfigViewModel { ID = 1, Name = "Service", Category = "Service", Function = "Add", Output = "Start", Type = "Type" });

                    HttpContext.Current.Cache["config"] = Configuration;
                }
                else
                {


                }

            }
            return (List<ConfigViewModel>)HttpContext.Current.Cache["config"];
        }

        public void EditConfig(JObject Data)
        {
            
            dynamic json = Data;

            json = json.data;

            Configuration.ID = json.ID;
            Configuration.Name = json.Name;
            Configuration.Function = json.Function;
            Configuration.Category = json.Category;
            Configuration.Type = json.Type;
            Configuration.Output = json.Output;

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = Convert.ToString(json.Parameter);

            List<dynamic> jMaths = (List<dynamic>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<dynamic>));
            
            Configuration.Parameter = jMaths;

        }

        public void Calculate()
        {




        }

        public void UpdateConfig(List<ConfigViewModel> Config)
        {
            HttpContext.Current.Cache["config"] = Config;
        }


    }




  
}