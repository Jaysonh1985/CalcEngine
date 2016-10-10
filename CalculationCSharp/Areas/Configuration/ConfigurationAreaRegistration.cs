// Copyright (c) 2016 Project AIM
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Configuration
{
    public class ConfigurationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Configuration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Configuration_default",
                "Configuration/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}