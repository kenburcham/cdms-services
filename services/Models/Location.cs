using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace services.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }

        public int LocationTypeId { get; set; }
        public int? WaterBodyId { get; set; }
        public int? SdeFeatureClassId { get; set; }
        public int? SdeObjectId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int? UserId { get; set; } //ok if this isn't set


        public int? Elevation { get; set; }

        public int Status { get; set; }

        //note: all decimal precision is set in the ServicesContext onmodelbuilding for EFF 5
        public decimal? GPSEasting { get; set; }
        public decimal? GPSNorthing { get; set; }
        public string Projection { get; set; }
        public string UTMZone { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public string OtherAgencyId { get; set; }
        public string ImageLink { get; set; }

        //these are very specific to water locations... is there a better way to abstract this?
        public float? WettedWidth { get; set; }
        public float? WettedDepth { get; set; }
        public decimal? RiverMile { get; set; }


        [JsonIgnore]
        public virtual List<Project> Projects { get; set; }

        public virtual LocationType LocationType { get; set; }
        public virtual SdeFeatureClass SdeFeatureClass { get; set; }
        
        public virtual WaterBody WaterBody { get; set; }

        public Location()
        {
            //set some defaults for our constructor
            LocationTypeId = LocationType.DEFAULT_LOCATIONTYPEID;
            CreateDateTime = DateTime.Now;
            SdeFeatureClassId = SdeFeatureClass.DEFAULT_FEATURECLASSID;

        }
    }

    public class WaterBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
//        public int? SdeObjectId { get; set; }

    }
}
