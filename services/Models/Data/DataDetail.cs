using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models.Data
{
    public abstract class DataDetail
    {
        public const int ROWSTATUS_ACTIVE = 0;
        public const int ROWSTATUS_DELETED = 1;

        public int Id { get; set; }
        public int RowId { get; set; }
        public int RowStatusId { get; set; } //0 = default/active; 1 = deleted
        public int ActivityId { get; set; }
        public int ByUserId { get; set; }
        public int QAStatusId { get; set; }
        public DateTime EffDt { get; set; }

        [JsonIgnore]
        public virtual Activity Activity { get; set; }

        [JsonIgnore]
        public virtual User ByUser { get; set; }

        [JsonIgnore]
        public virtual QAStatus QAStatus { get; set; }

    }
}