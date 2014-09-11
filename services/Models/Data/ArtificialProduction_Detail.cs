using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class ArtificialProduction_Detail: DataDetail
    {
        public string RunYear { get; set; }
        public string Species { get; set; }
        public string Origin { get; set; }
        public string Sex { get; set; }
        public string Disposition { get; set; }
        public int? TotalFishRepresented { get; set; }
        public string LifeStage { get; set; }
        public string FinClip { get; set; }
        public string Tag { get; set; }
        public int? NumberEggsTaken { get; set; }
        public string ReleaseSite { get; set; }
        public string FishComments { get; set; }
        public string ProgramGroup { get; set; }
    }
}