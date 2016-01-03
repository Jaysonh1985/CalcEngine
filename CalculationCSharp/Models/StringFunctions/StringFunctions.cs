using CalculationCSharp.Areas.Fire2006.Models;
using CalculationCSharp.Models.Calculation;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace CalculationCSharp.Models.StringFunctions
{
    public class StringFunctions
    {
        public string BulkCalc(List<OutputList> Obj, int HeaderRow, StringBuilder stringBuilder)
        {
                      
               if (HeaderRow < 1)
                {
                    foreach (var output in Obj)
                    {
                        stringBuilder.Append(output.ID);
                        stringBuilder.Append(",");
                    }
                    stringBuilder.AppendLine();

                    foreach (var output in Obj)
                    {
                        stringBuilder.Append(output.Field);
                        stringBuilder.Append(",");
                    }
                    stringBuilder.AppendLine();

                  
                }


                foreach (var output in Obj)
                {
                    stringBuilder.Append(output.Value);
                    stringBuilder.Append(",");
                }
                stringBuilder.AppendLine();


            return stringBuilder.ToString();

            }

    }

}
