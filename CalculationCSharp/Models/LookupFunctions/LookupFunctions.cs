// Copyright (c) 2016 Project AIM
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

    /// <summary>Lookup CSV on Server Map Path.
    /// </summary>

    /// <summary>CSV lookup queries a CSV on the Map Path of the server to find the correct factor table and returns a single value.
    /// <para>Tablename = Tablename of CSV file</para>
    /// <para>LookupValue = Value for the lookup</para>
    /// <para>DataType = DataType for lookup value</para>
    /// <para>ColumnNo = column number to return</para>
    /// </summary>
    public double CSVLookup(string Tablename, string LookupValue, int DataType, int ColumnNo)
	{
		string Name;
		string path = HttpContext.Current.Server.MapPath("\\Factor Tables\\");
        System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + path + "; Extended Properties='text; HDR=No; FMT=Delimited'");
		OleDbCommand command = con.CreateCommand();
        string Column = Convert.ToString("F" + ColumnNo);
        if (DataType == 1) {
            //Date
            command.CommandText = "SELECT top 1 " + Column + " FROM " + Tablename + ".csv " + " where F1 <= @date order by F1 desc";
            command.Parameters.AddWithValue("@date", LookupValue);
        }
        else if (DataType == 2)  {
            //String
            command.CommandText = "SELECT top 1 " + Column + " FROM " + Tablename + ".csv " + " where F1 = " + LookupValue;
        }
        else if (DataType == 3) {
            //Number
            command.CommandText = "SELECT top 1 " + Column + " FROM " + Tablename + ".csv " + " where F1 <= " + LookupValue + " order by F1 desc";
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
        if(nameObj == "")
        {
            return 0;
        }
        else
        {
            return Convert.ToDouble(nameObj);
        }
   	}
    /// <summary>CSV column number to match a value lookup horizontally to match the value in the CSV.
    /// <para>Tablename = Tablename of CSV file</para>
    /// <para>RowNo = row to l</para>
    /// <para>LookupValue = Lookup value</para>
    /// </summary>
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