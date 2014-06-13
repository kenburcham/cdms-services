using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class FishTransport_Detail : DataDetail
    {
        public string ReleaseSite { get; set; }
        public int? TotalFishRepresented { get; set; }
        public string ReleaseSiteComments { get; set; }
        public string TransportTankUnit { get; set; }
        public double? TransportReleaseTemperature { get; set; }
        public double? TransportReleaseTemperatureF { get; set; }
        public int? TransportMortality { get; set; }
    }
}