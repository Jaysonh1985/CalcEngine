// Copyright (c) 2016 Project AIM
using System.Web;
using System.Web.Optimization;

namespace CalculationCSharp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/toastr.css",
                      "~/Content/angular-ui-tree.css",
                       "~/Areas/Configuration/Content/calcbuilder.css"
                      ));
            //Create bundel for jQueryUI  
            //js  
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui-{version}.js"));
            //css  
            bundles.Add(new StyleBundle("~/Content/cssjqryUi").Include(
                   "~/Content/jquery-ui.css"));
            //Calculation Configuration
            bundles.Add(new ScriptBundle("~/bundles/calculationConfiguration").Include(
                        "~/Areas/Configuration/Scripts/ConfigFunctions/Logic/groupCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigFunctions/Logic/logicCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigBuilder/configBuilderColumnCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigBuilder/configBuilderUICtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigBuilder/configCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigBuilder/configCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigMenu/configMenuCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigMenu/configMenuAddCalcCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigFunctions/History/historyCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigFunctions/Regression/regressionCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigFunctions/Regression/regressionInputCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigFunctions/Regression/regressionDifferenceCtrl.js",
                        "~/Areas/Configuration/Scripts/ConfigFunctions/Regression/regressionOutputCtrl.js",
                        "~/Areas/Configuration/Scripts/Directives/CustomValidations.js",
                        "~/Areas/Configuration/Scripts/ConfigFunctions/Impact Assessment/impactAssessmentCtrl.js",
                        "~/Areas/Configuration/Scripts/Factories/configFunctionFactory.js",
                        "~/Areas/Configuration/Scripts/Factories/configValidationFactory.js",
                        "~/Areas/Configuration/Scripts/Factories/configTypeaheadFactory.js",
                        "~/Areas/Configuration/Scripts/Services/configService.js",
                        "~/Areas/Configuration/Scripts/Services/configMenuService.js",
                        "~/Areas/Configuration/Scripts/Services/configHistoryService.js",
                        "~/Areas/Configuration/Scripts/Services/configRegressionService.js",
                        "~/Areas/Configuration/Scripts/ConfigFunctions/Table/tableCtrl.js",
                        "~/Areas/Dashboard/Scripts/Controllers/DashboardCalcCtrl.js",
                        "~/Areas/Dashboard/Scripts/Controllers/DashboardFunctionCtrl.js",
                        "~/Areas/Dashboard/Scripts/Services/DashboardService.js",
                        "~/Areas/Dashboard/Scripts/Controllers/DashboardTeamCtrl.js",
                        "~/Areas/Dashboard/Scripts/Controllers/AddTeamCalcCtrl.js"
                        ));

            //Calculation Configuration
            bundles.Add(new ScriptBundle("~/bundles/projectBoard").Include(
                        "~/Areas/Project/Scripts/kanbanBoardDragDrop.js",
                        "~/Areas/Project/Scripts/boardService.js",
                        "~/Areas/Project/Scripts/boardCtrl.js",
                        "~/Areas/Project/Scripts/menuCtrl.js" ,
                        "~/Areas/Project/Scripts/storyCtrl.js",
                        "~/Areas/Project/Scripts/reportCtrl.js",
                        "~/Areas/Project/Scripts/projectMenuAddCalcCtrl.js",
                        "~/Areas/Project/Scripts/Factories/fileUploadService.js"
                        ));
            //Calculation Configuration
            bundles.Add(new ScriptBundle("~/bundles/Calculation").Include(
                        "~/Areas/Calculation/Scripts/calculationCtrl.js",
                        "~/Areas/Calculation/Scripts/calculationService.js",
                        "~/Areas/Calculation/Scripts/calculationMenuCtrl.js"
                        ));

            //Turns off bootstrap.min
            BundleTable.EnableOptimizations = false;
            

        }
    }
}
