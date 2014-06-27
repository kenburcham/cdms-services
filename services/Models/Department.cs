using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace services.Models
{
    public class Department
    {
        public const int DEFAULT_DEPARTMENT = 1;

        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
