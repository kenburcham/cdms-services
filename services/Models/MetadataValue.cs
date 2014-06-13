using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    public class MetadataValue
    {
        public  int Id { get; set; }
        public int MetadataPropertyId { get; set; }
        public int RelationId { get; set; } //the ID of the actual entity we're related to (projectid = 44)

        public DateTime EffDt { get; set; }
        public string Values { get; set; }
        public int UserId { get; set; }


    }
}