// Copyright (c) 2016 Project AIM
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Calculation
{
    public class CalculationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Calculation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Calculation_default",
                "Calculation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}