using System.Web.Mvc;

namespace CalculationCSharp.Areas.Fire2006
{
    public class Fire2006AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Fire2006";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Fire2006_default",
                "Fire2006/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}