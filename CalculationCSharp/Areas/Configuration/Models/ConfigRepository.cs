// Copyright (c) 2016 Project AIM
using CalculationCSharp.Areas.Configuration.Models;
using CalculationCSharp.Areas.Configuration.Models.Actions;
using CalculationCSharp.Models.Calculation;
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
        /// <summary>Sets the updated configuration in the browser cache, if no config is available then return the input column only
        /// <para>Config = JSON config </para>
        /// </summary>
        public List<CategoryViewModel> GetConfig(CategoryViewModel Config)
        {
            var Configuration = new List<CategoryViewModel>();
            if (HttpContext.Current.Cache["config"] == null)
            {
                if (Config == null)
                {
                    Configuration.Add(new CategoryViewModel { ID = 0, Name = "Input", Results = new List<ResultViewModel>() });
                    HttpContext.Current.Cache["config"] = Configuration;
                }
            }
            return (List<CategoryViewModel>)HttpContext.Current.Cache["config"];
        }
        /// <summary>Updates browser Cache on change of config
        /// <para>Config = JSON config </para>
        /// </summary>
        public void UpdateConfig(List<CategoryViewModel> Config)
        {
            HttpContext.Current.Cache["config"] = Config;
        }
    } 
}