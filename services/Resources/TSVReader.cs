using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;
using Newtonsoft.Json.Linq;
using NLog;

namespace services.Resources
{
    /**
     * Reads a TSV (tab separated) file and return
     */ 
    public class TSVReader
    {
        private string fileName;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TSVReader(string a_fileName)
        {
            this.fileName = a_fileName;


        }

        internal ImportDataResult getImportDataResult(int start_on_line = 1)
        {
            ImportDataResult dataresult = new ImportDataResult();
            var rows = new List<JObject>();
            var num_to_skip = start_on_line - 1;

            List<string> columns = new List<string>();

            using (var reader = new StreamReader(fileName))
            {
                for (int i = 0; i < num_to_skip; i++)
                {
                    reader.ReadLine(); //skip one line.    
                }


                string[] headers = reader.ReadLine().Split('\t');
                foreach (string header in headers)
                {
                    string header_val = header;
                    if (header == "" || header == null)
                        header_val = "-blank-";

                    columns.Add(header_val.Replace("\"",""));
                }

                dataresult.columns = columns;

                //now we have the columns... lets get the rows!
                string wholeline;
                string[] line;

                while ((wholeline = reader.ReadLine()) != null) //NOTE: reader.EndOfStream does NOT work here. :(
                {
                    //logger.Debug(wholeline);
                    line = wholeline.Split('\t');
                    var idx = 0;
                    var row = new JObject();

                    //iterate our columns and copy
                    foreach (var column in columns)
                    {
                        row.Add(new JProperty(column, line[idx]));
                        idx++;
                    }

                    rows.Add(row);
                }

                dataresult.rows = rows;
            }

            return dataresult;
        }

        


    }
}