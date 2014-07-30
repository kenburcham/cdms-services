using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class Appraisal_Detail: DataDetail
    {
        public string AppraisalYear { get; set; }
        public string AppraisalFiles{ get; set; }
        public string AppraisalPhotos { get; set; }
        public string AppraisalComments { get; set; }
        public string AppraisalStatus { get; set; }
        public string AppraisalType { get; set; }
        public string AppraisalLogNumber { get; set; }
        public int? AppraisalValue { get; set; }
        public DateTime? AppraisalValuationDate { get; set; }
        public string Appraiser { get; set; }
        public string TypeOfTransaction { get; set; }
        public string PartiesInvolved { get; set; }
        public string AppraisalProjectType { get; set; }

    }
}