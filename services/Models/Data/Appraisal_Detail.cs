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

    }
}