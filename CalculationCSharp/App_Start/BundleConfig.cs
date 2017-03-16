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
                      "~/Content/angular-ui-tree.css"
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
                        "~/Areas/Configuration/Scripts/Group/groupCtrl.js",
                        "~/Areas/Configuration/Scripts/Logic/logicCtrl.js",
                        "~/Areas/Configuration/Scripts/Config/configCtrl.js",
                        "~/Areas/Configuration/Scripts/Config/configService.js",
                        "~/Areas/Configuration/Scripts/Config/configMenuCtrl.js",
                        "~/Areas/Configuration/Scripts/Comments/commentsCtrl.js",
                        "~/Areas/Configuration/Scripts/Date Part/datePartCtrl.js",
                        "~/Areas/Configuration/Scripts/CalculationMenu/configMenuAddCalcCtrl.js",
                        "~/Areas/Configuration/Scripts/History/historyCtrl.js",
                        "~/Areas/Configuration/Scripts/Regression/regressionCtrl.js",
                        "~/Areas/Configuration/Scripts/Regression/regressionInputCtrl.js",
                        "~/Areas/Configuration/Scripts/Regression/regressionDifferenceCtrl.js",
                        "~/Areas/Configuration/Scripts/Regression/regressionOutputCtrl.js",
                        "~/Areas/Configuration/Scripts/Custom Validations/CustomValidations.js",
                        "~/Areas/Configuration/Scripts/Function/functionCtrl.js",
                        "~/Areas/Configuration/Scripts/Impact Assessment/impactAssessmentCtrl.js",
                        "~/Areas/Configuration/Scripts/Config/Factories/configFunctionFactory.js",
                        "~/Areas/Configuration/Scripts/Config/Factories/configModalFactory.js",
                        "~/Areas/Configuration/Scripts/Config/Factories/configValidationFactory.js",
                        "~/Areas/Configuration/Scripts/Config/Factories/configTypeaheadFactory.js"
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
