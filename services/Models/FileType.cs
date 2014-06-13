using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace services.Models
{
    public class FileType
    {
        public int Id { get; set; }
        public string Name { get; set; } //PDF
        public string Description { get; set; } //pdf
        public string Extensions { get; set; }

        public const int UNKNOWN = 5;

        private static Logger logger = LogManager.GetCurrentClassLogger();


        //returns the FileTypeId we think you'll want for this filename
        //note: we expect the filename to be simple: "somefile.pdf"
        public static int getFileTypeFromFilename(string filename)
        {
            return FileType.getFileTypeFromFilename(new System.IO.FileInfo(filename));
        }

        public static int getFileTypeFromFilename(System.IO.FileInfo fileinfo)
        {
            logger.Debug("Looking up filetype for " + fileinfo.Extension);
            var result = FileType.UNKNOWN;
            var db = ServicesContext.Current;
            FileType match = db.FileTypes.Where(o => o.Extensions.Contains(fileinfo.Extension.ToLower().Substring(1))).FirstOrDefault();
            if (match != null)
                result = match.Id;

            logger.Debug("filetype we matched : " + result);

            return result;
        }



    }

    
}