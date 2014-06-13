using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    public class LocationType
    {
        public const int PROJECT_TYPE = 3;
        public const int DATA_TYPE = 2;
        public const int DEFAULT_LOCATIONTYPEID = 1;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}