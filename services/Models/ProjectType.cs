using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    public class ProjectType
    {
        public const int DEFAULT_PROJECT_TYPE = 15;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}