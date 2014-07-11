using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    //This roughly maps to a concept of "Allotment" -- a container for appraisals in our appraisal dataset.
    public class Appraisal_Header: DataHeader
    {
        public string Allotment { get; set; }
        public string AllotmentStatus { get; set; }
        public string AllotmentName { get; set; }
        public string AllotmentDescription { get; set; }
        public string AllotmentComments { get; set; }
        public string CobellAppraisalWave { get; set;  }
        public string LeaseTypes { get; set; }
        public string MapFiles { get; set; }
        public string TSRFiles { get; set; }
        public string FarmingLeaseFiles { get; set; }
        public string TimberAppraisalFiles { get; set; }
        public string GrazingLeaseFiles { get; set; }
        public string AllotmentPhotoFiles { get; set; }
        public string RegionalOfficeReviewFiles { get; set; }

    }
}