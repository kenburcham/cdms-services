using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using services.Models;

namespace services.Resources
{
    //used on import preview use case
    public class ImportDataResult
    {
        public IEnumerable<String> columns;
        public Object rows;
    }

    //used on import final use case
    public class ImportResult
    {
        public List<Activity> duplicates;
        public IEnumerable<String> errors;
        public Boolean success;
    }

    public class ExportResult
    {
        public string file;
        public IEnumerable<String> errors;
        public Boolean success;
    }

}