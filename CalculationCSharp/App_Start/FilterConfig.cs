// Copyright (c) 2016 Project AIM
using System.Web;
using System.Web.Mvc;

namespace CalculationCSharp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
