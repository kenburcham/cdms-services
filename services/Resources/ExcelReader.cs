using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;

//Ken Burcham 1/30/2014

namespace services.Resources
{
    //basics learned here: http://typecastexception.com/post/2013/10/17/Use-Cross-PlatformOSS-ExcelDataReader-to-Read-Excel-Files-with-No-Dependencies-on-Office-or-ACE.aspx
    public class ExcelReader
    {
        string _path;
        IExcelDataReader reader;

        public ExcelReader(string path)
        {
            _path = path;
            reader = getExcelReader();
        }

        public IExcelDataReader getExcelReader()
        {
            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
            // to get started. This is how we avoid dependencies on ACE or Interop:
            FileStream stream = File.Open(_path, FileMode.Open, FileAccess.Read);

            // We return the interface, so that 
            IExcelDataReader reader = null;
            try
            {
                if (_path.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                if (_path.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<string> getWorksheetNames()
        {
            var workbook = reader.AsDataSet();
            var sheets = from DataTable sheet in workbook.Tables select sheet.TableName;
            return sheets;
        }


        public IEnumerable<DataRow> getData(string sheet = null, bool firstRowIsColumnNames = true)
        {
            if (sheet == null)
            {
                sheet = getWorksheetNames().FirstOrDefault().ToString();
            }

            reader.IsFirstRowAsColumnNames = firstRowIsColumnNames;
            var workSheet = reader.AsDataSet().Tables[sheet];
            var rows = from DataRow a in workSheet.Rows select a;

            return rows;
        }

        public IEnumerable<String> getColumns(string sheet = null, bool firstRowIsColumnNames = true)
        {
            if (sheet == null)
            {
                sheet = getWorksheetNames().FirstOrDefault().ToString();
            }

            reader.IsFirstRowAsColumnNames = firstRowIsColumnNames;
            var workSheet = reader.AsDataSet().Tables[sheet];
            
            var column_list = new List<String>();

            foreach (DataColumn column in workSheet.Columns)
	        {
                column_list.Add(column.ColumnName);
	        }

            return column_list;
            
        }

        public void close()
        {
            reader.Close();
        }


    }
}