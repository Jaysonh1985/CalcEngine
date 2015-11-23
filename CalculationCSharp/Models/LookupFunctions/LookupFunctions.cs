using System;
using System.Data.OleDb;
using System.Web;

public class LookupFunctions
{

	public object server { get; set; }
	public object Request { get; set; }

	public double CSVLookup(string Tablename, string LookupValue, int DataType, int ColumnNo)
	{
		string Name;
		string path = HttpContext.Current.Server.MapPath("\\Factor Tables\\");
        System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + path + "; Extended Properties='text; HDR=No; FMT=Delimited'");
		OleDbCommand command = con.CreateCommand();

        string Column = Convert.ToString("F" + ColumnNo);

     	if (DataType == 1) {
			//Date
			command.CommandText = "SELECT top 1 " + Column + " FROM " + Tablename + ".csv " + " where F1 <= " + "@Date" + " order by F1 desc";
		} else if (DataType == 2) {
			//String
			command.CommandText = "SELECT top 1 " + Column + " FROM " + Tablename + ".csv " + " where F1 = " + "@Date";
		}

       
		command.Parameters.AddWithValue("@Value", LookupValue);
		con.Open();
        OleDbDataReader reader = command.ExecuteReader();
        object nameObj = null;
        while (reader.Read())
        {
            nameObj = reader[0].ToString();
        }
        con.Close();
        if ((nameObj != null))
        {
            Name = nameObj.ToString();
        }

        return Convert.ToDouble(nameObj);

	}



    
	//public object Vlookup(string id, Microsoft.Office.Interop.Excel.Application xlapp, Microsoft.Office.Interop.Excel.Workbook xlbook, string Sheets, int Column, bool Match, bool isdate)
	//{

 //       Microsoft.Office.Interop.Excel.Worksheet xlSheet;
 //       Microsoft.Office.Interop.Excel.Range xlRange;
	//	string VlResult = ("");

	//	xlSheet = xlbook.Worksheets(Sheets);
	//	xlRange = xlSheet.UsedRange;

	//	if (isdate == true) {
	//		string DateText;
	//		DateTime ConvertedDateTest;
	//		int oadays;

	//		DateText = id;
	//		ConvertedDateTest = DateTime.Parse(DateText);
	//		oadays = ConvertedDateTest.ToOADate;

	//		try {
	//			VlResult = xlapp.WorksheetFunction.VLookup(oadays, xlRange, Column, Match);

	//		} catch (Exception ex) {
	//		}


	//	} else {
	//		try {
	//			VlResult = xlapp.WorksheetFunction.VLookup(id, xlRange, Column, Match);

	//		} catch (Exception ex) {
	//		}

	//	}

	//	return VlResult;

	//}

}