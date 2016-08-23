using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
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
			command.CommandText = "SELECT top 1 " + Column + " FROM " + Tablename + ".csv " + " where F1 <= " + LookupValue + " order by F1 desc";
		} else if (DataType == 2) {
			//String
			command.CommandText = "SELECT top 1 " + Column + " FROM " + Tablename + ".csv " + " where F1 = " + LookupValue;
		}

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

    public int CSVColumnNumber(string Tablename, int RowNo, string LookupValue)
    {

        var lines = File.ReadAllLines(HttpContext.Current.Server.MapPath("\\Factor Tables\\" + Tablename + ".csv"));

        List<string> Tables = File.ReadAllLines(HttpContext.Current.Server.MapPath("\\Factor Tables\\" + Tablename + ".csv")).ToList();

        //Number of rows columns
        List<string> columnNo = lines[RowNo].Split(',').ToList();

        var col = columnNo.IndexOf(LookupValue) + 1;
        

        return col; 
    }

}