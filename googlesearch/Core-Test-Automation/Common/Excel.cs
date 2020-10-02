using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace Core_Test_Automation.Common
{
    /// <summary>
    /// Contains all functions for Excel access.
    /// </summary>
    public class Excel
    {
        private string filePath;
        FileStream file;
        //FileStream fileout;
        private static XSSFWorkbook excelWBook;
        private static ISheet excelWSheet;
        private static ICell cell;
        private static IRow row;

        static IWorkbook workbook;

        public Excel(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Initializes Excel file.
        /// </summary>
        public void SetExcelFile(FileAccess access)
        {
            try
            {
                file = new FileStream(filePath, FileMode.Open, access);
                excelWBook = new XSSFWorkbook(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Excel read error," + System.Environment.NewLine + ex.Message);
            }
        }

        public void CloseExcelFile(FileAccess access)
        {
            try
            {
                if (!access.Equals(FileAccess.Read))
                {
                    excelWBook.Write(file);
                }
                file.Flush();
                file.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Excel close error," + System.Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Get cell value on a sheet in an Excel file.
        /// </summary>
        /// <param name="sheetName">Sheet name where the cell is located.</param>
        /// <param name="cellName">Cell address.</param>
        /// <returns></returns>
        public string Get(string sheetName, string cellName)
        {
            using (FileStream rstr = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(rstr);
                excelWSheet = workbook.GetSheet(sheetName);

                var cr = new CellReference(cellName);
                row = excelWSheet.GetRow(cr.Row);
                cell = row.GetCell(cr.Col);

                rstr.Close();

                return cell.StringCellValue;
            }
        }

        /// <summary>
        /// Get cell value on a sheet in an Excel file using column number and row number.
        /// </summary>
        /// <param name="sheetName">Sheet name where the cell is located.</param>
        /// <param name="colNum">Column number of cell.</param>
        /// <param name="rowNum">Row number of cell.</param>
        /// <returns></returns>
        public string Get(string sheetName, int colNum, int rowNum)
        {
            using (FileStream rstr = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(rstr);
                excelWSheet = workbook.GetSheet(sheetName);

                row = excelWSheet.GetRow(rowNum - 1);
                cell = row.GetCell(colNum - 1);

                String cellData = null;
                DataFormatter formatter = new DataFormatter();
                cellData = formatter.FormatCellValue(cell);

                rstr.Close();

                return cellData;
            }
        }

        /// <summary>
        /// Get cell value on a sheet in an Excel file using column number and row number.
        /// </summary>
        /// <param name="sheetName">Sheet name where the cell is located.</param>
        /// <param name="columnName">Column name of cell.</param>
        /// <returns></returns>
        public string GetUsingColumnName(string sheetName, string columnName)
        {
            string cellData = "startCol";
            int colNum = 1;
            int rowNum = 2;
            bool columnFound = false;
            bool rowFound = false;
            
            using (FileStream rstr = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(rstr);
                excelWSheet = workbook.GetSheet(sheetName);
                //DataFormatter formatter = new DataFormatter();

                //Get column number that matches the column Name
                while (!cellData.Equals(columnName) && !cellData.Equals(""))
                {
                    try
                    {
                        row = excelWSheet.GetRow(0);
                        cell = row.GetCell(colNum - 1);
                        cellData = cell.StringCellValue;
                        if (cellData.Equals(columnName))
                        {
                            columnFound = true;
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Column not found: " + columnName + System.Environment.NewLine + e);
                    }

                    colNum++;
                }

                cellData = "startRow";

                //Get row number that matches scenario name
                if (columnFound)
                {
                    while (!cellData.Equals(BaseTest.scenarioName) && !cellData.Equals(""))
                    {
                        try
                        {
                            row = excelWSheet.GetRow(rowNum - 1);
                            cell = row.GetCell(0);
                            cellData = cell.StringCellValue;
                            if (cellData.Equals(BaseTest.scenarioName))
                            {
                                rowFound = true;
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Scenario not found for test data: " + BaseTest.scenarioName + System.Environment.NewLine + e);
                        }

                        rowNum++;
                    }

                    if (rowFound)
                    {
                        try
                        {
                            row = excelWSheet.GetRow(rowNum - 1);
                            cell = row.GetCell(colNum - 1);
                            cellData = cell.StringCellValue;
                        }
                        catch
                        {
                            Console.WriteLine("Empty cell: " + columnName);
                            cellData = "";
                        }
                    }
                }
                else
                {
                    throw new Exception("Data not found: " + columnName);
                }
                
                rstr.Close();
            }

            return cellData;
        }

        /// <summary>
        /// Writes cell value on a sheet in an Excel file.
        /// </summary>
        /// <param name="sheetName">Sheet name where the cell is located.</param>
        /// <param name="cellName">Cell address.</param>
        /// <param name="cellValue">Value to write in cell.</param>
        public void CreateExcel(string sheetName, string cellName, string cellValue)
        {
            if (!File.Exists(filePath))
            {
                using (FileStream str = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook = new XSSFWorkbook();
                    excelWSheet = workbook.CreateSheet(sheetName);
                    var cr = new CellReference(cellName);
                    row = excelWSheet.GetRow(cr.Row);
                    cell = row.GetCell(cr.Col);
                    cell.SetCellValue(cellValue);
                    workbook.Write(str);
                    str.Flush();
                    str.Close();
                }
            }
            else
            {
                using (FileStream rstr = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(rstr);
                    excelWSheet = workbook.GetSheet(sheetName);

                    using (FileStream wstr = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        var cr = new CellReference(cellName);
                        row = excelWSheet.GetRow(cr.Row);
                        cell = row.GetCell(cr.Col);
                        cell.SetCellValue(cellValue);
                        //Debug.Print(cell.ToString());
                        workbook.Write(wstr);
                        wstr.Flush();
                        wstr.Close();
                    }
                    rstr.Close();
                }
            }
        }

        /// <summary>
        /// Writes cell value on a sheet in an Excel file.
        /// </summary>
        /// <param name="sheetName">Sheet name where the cell is located.</param>
        /// <param name="colNum">Column number of cell.</param>
        /// <param name="rowNum">Row number of cell.</param>
        /// <param name="cellValue">Value to write in cell.</param>
        public void CreateExcel(string sheetName, int colNum, int rowNum, string cellValue)
        {
            if (!File.Exists(filePath))
            {
                using (FileStream str = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook = new XSSFWorkbook();
                    excelWSheet = workbook.CreateSheet(sheetName);
                    row = excelWSheet.GetRow(rowNum - 1);
                    cell = row.GetCell(colNum - 1);
                    cell.SetCellValue(cellValue);
                    workbook.Write(str);
                    str.Flush();
                    str.Close();
                }
            }
            else
            {
                using (FileStream rstr = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(rstr);
                    excelWSheet = workbook.GetSheet(sheetName);

                    using (FileStream wstr = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        row = excelWSheet.GetRow(rowNum - 1);
                        cell = row.GetCell(colNum - 1);
                        cell.SetCellValue(cellValue);
                        //Debug.Print(cell.ToString());
                        workbook.Write(wstr);
                        wstr.Flush();
                        wstr.Close();
                    }
                    rstr.Close();
                }
            }
        }
    }
}