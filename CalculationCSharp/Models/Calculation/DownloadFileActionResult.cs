using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CalculationCSharp.Models.Calculation
{
    public class DownloadFileActionResult : ActionResult
    {

        public List<OutputList> ExcelGridView { get; set; }
        public string fileName { get; set; }


        public DownloadFileActionResult(List<OutputList> gv, string pFileName)
        {
            ExcelGridView = gv;
            fileName = pFileName;
        }


        public override void ExecuteResult(ControllerContext context)
        {

            var grid = new System.Web.UI.WebControls.GridView();

            grid.DataSource = from data in ExcelGridView
                              select new
                              {
                                  ID = data.ID,
                                  Field = data.Field,
                                  Value = data.Value

                              };

            grid.DataBind();

            //Create a response stream to create and write the Excel file
            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            curContext.Response.Charset = "";
            curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            curContext.Response.ContentType = "application/vnd.ms-excel";

            //Convert the rendering of the gridview to a string representation 
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);

            //Open a memory stream that you can use to write back to the response
            byte[] byteArray = Encoding.ASCII.GetBytes(sw.ToString());
            MemoryStream s = new MemoryStream(byteArray);
            StreamReader sr = new StreamReader(s, Encoding.ASCII);

            //Write the stream back to the response
            curContext.Response.Write(sr.ReadToEnd());
            curContext.Response.End();

        }

    }
}