using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace services.Models
{

    public class SdeFeatureClass
    {
        public const int DEFAULT_FEATURECLASSID = 1;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Database { get; set; }
        public string Service { get; set; }
        public bool IsActive { get; set; }

    }
}
