using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace services.Models.Data
{
    public class CreelSurvey_Carcass : DataDetailRelation
    {
        public string Species { get; set; }
        public string MethodCaught { get; set; }
        public string Disposition { get; set; }
        public string Sex { get; set; }
        public string Origin { get; set; }
        public string FinClips { get; set; }
        public string Marks { get; set; }
        public int? ForkLength { get; set; }
        public int? MeHPLength { get; set; }
        public string SnoutId { get; set; }
        public string ScaleId { get; set; }
        public string CarcassComments { get; set; }
        
    }
}