using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models
{
    public class MetadataProperty
    {
        public int Id { get; set; }
        public int MetadataEntityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DataType { get; set; }
        public string ControlType { get; set; }
        public string PossibleValues { get; set; }

        [JsonIgnore]
        public virtual MetadataEntity MetadataEntity { get; set; }
    }
}