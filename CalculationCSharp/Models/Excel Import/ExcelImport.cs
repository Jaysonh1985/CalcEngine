public class ExcelImport
{
    object ExcelImportArray()
    {

        Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
        Microsoft.Office.Interop.Excel.Workbook xlbook;
        Microsoft.Office.Interop.Excel.Worksheet xlsheet;
        Microsoft.Office.Interop.Excel.Range xlrange;

        xlbook = xlapp.Workbooks.Open("C:\\Users\\JaysonH\\Desktop\\Winter Calculation v0.2.xlsm", true);
        xlsheet = xlbook.Worksheets["Extract"];
		xlrange = xlsheet.UsedRange;

		object[,] myArray;
		//<-- declared as 2D Array
		myArray = xlrange.Value;
		//store the content of each cell

		for (int r = 1; r <= myArray.GetUpperBound(0); r++) {
			for (int c = 1; c <= myArray.GetUpperBound(1); c++) {
                object myValue = myArray.GetValue(c, r);
			}
		}
		return myArray;
	}
}