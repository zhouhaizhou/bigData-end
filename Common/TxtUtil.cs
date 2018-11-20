using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Common
{
    public  class TxtUtil
    {
        public  static void DataTableToTxt(DataTable tb, string fileName, string Delimiter = "\t")
        {
            string strFileContent = "";
            //循环表头
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                if (i > 0)
                {
                    strFileContent = strFileContent + Delimiter;
                }
                strFileContent = strFileContent + tb.Columns[i].ColumnName;
            }
            strFileContent = strFileContent + "\r\n";
            //循环表内容
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                for (int j = 0; j < tb.Columns.Count; j++)
                {
                    strFileContent = strFileContent + tb.Rows[i][j].ToString().Trim() + Delimiter;
                }
                strFileContent = strFileContent + "\r\n";
            }
            WriteFile(fileName, strFileContent);
        }
        public static   void WriteFile(string strFileName, string strFileContent)
        {
            FileStream fs = new FileStream(strFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
            sw.Write(strFileContent);
            sw.Close();
            fs.Close();
        }
    }
}
