using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public abstract class DataHeader
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int ByUserId { get; set; }
        public DateTime EffDt { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual User ByUser { get; set; }
    }
}