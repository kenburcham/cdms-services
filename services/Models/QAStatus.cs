using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models
{
    public class QAStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        //the following indicate to EntityFramework to make a many to many
        [JsonIgnore]
        public virtual List<Dataset> Datasets { get; set; } //Dataset Activity QA options

        [JsonIgnore]
        public virtual List<Dataset> QARowDatasets { get; set; } //Dataset Row QA options

    }
}