using Aspose.Cells;
using Re.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common
{
    public class ExcelUtil
    {

        /// <summary>
        /// Excel的内容读取到DataTable中
        /// </summary>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath)
        {
            try
            {
                Workbook workbook = new Workbook(filePath);
                //   workbook.Open(filePath);
                Cells cells = workbook.Worksheets[0].Cells;
                // System.Data.DataTable dataTable1 = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn);//noneTitle
                // System.Data.DataTable dataTable2 = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxColumn, true);//showTitle
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ExcelUtil_ExcelToDataTable " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "错误记录", recordInfo);
                throw ex;
            }
        }
        /// <summary>
        /// DataTable数据导出Excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filepath"></param>
        public static void DataTableToExcel(DataTable data, string filepath)
        {
            try
            {
                Workbook book = new Workbook();
                Worksheet sheet = book.Worksheets[0];
                Cells cells = sheet.Cells;
                int Colnum = data.Columns.Count;//表格列数 
                int Rownum = data.Rows.Count;//表格行数 
                //生成行 列名行 
                for (int i = 0; i < Colnum; i++)
                {
                    cells[0, i].PutValue(data.Columns[i].ColumnName);
                }
                //生成数据行 
                for (int i = 0; i < Rownum; i++)
                {
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[1 + i, k].PutValue(data.Rows[i][k].ToString());
                    }
                }
                book.Save(filepath);
                GC.Collect();
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ExcelUtil_DataTableToExcel " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "错误记录", recordInfo);
                throw ex;
            }
        }
    }
}
