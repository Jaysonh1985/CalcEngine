using CalculationCSharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CalculationCSharp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
       
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

            if (bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"]))
            {
                var configuration = new CalculationCSharp.Migrations.CalculationContext.ConfigurationB();
                var migrator = new DbMigrator(configuration);
                migrator.Update();
                var configurationApplication = new CalculationCSharp.Migrations.Identity.ConfigurationA();
                var migratorApplication = new DbMigrator(configurationApplication);
                migratorApplication.Update();
            }

        }
    }
}
