using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using System;
using CalculationCSharp.Models.Calculation;
using System.Collections.Generic;

namespace CalculationCSharp.Controllers
{
    

    public class CalculationController : Controller
    {
        public object List;

        [HttpGet]
        public ActionResult Index(string SchemeType, string CalculationType)
        {
            CalculationCSharp.Models.Calculation.Index IndexCalculation = new CalculationCSharp.Models.Calculation.Index();

            IndexCalculation.CalculationType = CalculationType;
            IndexCalculation.SchemeType = SchemeType;


            return RedirectToAction(CalculationType, SchemeType);
        }

         
        public ActionResult Output()
        {
            List = TempData["Data"];
     
            return View(List);

        }
        
        
        //public void ExportClientsListToCSV()
        //{
        //    List.Setup(InputForm);

        //    StringWriter sw = new StringWriter();

        //    sw.WriteLine("\"ID\",\"Field\",\"Value\"");

        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", "attachment;filename=Excel.csv");
        //    Response.ContentType = "text/csv";


        //    foreach (var line in List.List)
        //    {
        //        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"",
        //                                   line.ID,
        //                                   line.Field,
        //                                   line.Value));
        //    }
        //    Response.Write(sw.ToString());

        //    Response.End();

        //}


        //public void ExportClientsListToExcel()
        //{
        //    List.Setup(InputForm);
        //    var grid = new System.Web.UI.WebControls.GridView();

        //    grid.DataSource = from data in List.List
        //                      select new
        //                      {
        //                          ID = data.ID,
        //                          Field = data.Field,
        //                          Value = data.Value

        //                      };

        //    grid.DataBind();

        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", "attachment; filename=Excel.xls");
        //    Response.ContentType = "application/excel";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);

        //    grid.RenderControl(htw);

        //    Response.Write(sw.ToString());

        //    Response.End();

        //}
    }
}