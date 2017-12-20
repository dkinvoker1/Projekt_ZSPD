using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ZSPD.Models
{
    public class ExcelModel
    {
  
        public static DataTable readExcel(ExcelPackage pck)
        {

                ExcelWorksheet worksheet = pck.Workbook.Worksheets.First();
                DataTable dt = new DataTable();
                foreach (var header in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    dt.Columns.Add(header.Text);
                }
                for (var numRow = 2; numRow <= worksheet.Dimension.End.Row; numRow++)
                {
                    var row = worksheet.Cells[numRow, 1, numRow, worksheet.Dimension.End.Column];
                    var newRow = dt.NewRow();
               
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                { 
                   newRow[0] = worksheet.Cells[numRow, col].Text; 
                    
                }
                dt.Rows.Add(newRow);
            }
                return dt;
          
        }
    }
}