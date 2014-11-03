using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using services.Models;

namespace services.Models.Data
{
    //has all fields used by both Phone and Field interviews...
    public class CreelSurvey_Detail: DataDetail
    {
        public string InterviewTime { get; set; }
        public string FishermanName { get; set; }
        public string FishermanPhone { get; set; }
        public int? HoursFished { get; set; }
        public int? MinutesFished { get; set; }
        public int? NumberFishCaught { get; set; }
        public int? NumberFishKept { get; set; }

        public int? TotalFishingTrips { get; set; }
        public int? TotalCreelInterviews { get; set; }

        public string InterviewComments { get; set; }

        //note: all decimal precision is set in the ServicesContext onmodelbuilding for EFF 5
        public decimal? GPSEasting { get; set; }
        public decimal? GPSNorthing { get; set; }
        public string Projection { get; set; }
        public string UTMZone { get; set; }

        //this gets its own specific location -- built auto-magically from teh GPS-Easting/GPS-Northing (on the UI side)
        public int? DetailLocationId { get; set; }
        public virtual Location DetailLocation { get; set; }

        public string WaterBody { get; set; } //this is a lookup, so we just store the matching label.
        public string SectionNumber { get; set; }

    }
}