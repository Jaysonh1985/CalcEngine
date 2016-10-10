// Copyright (c) 2016 Project AIM
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Project
{
    public class ProjectAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Project";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Project_default",
                "Project/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}