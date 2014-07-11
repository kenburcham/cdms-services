using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class WaterTemp_Header : DataHeader
    {
        public string Technicians { get; set; }
        public string Comments { get; set; }
        public string CollectionType { get; set; }
        public double? DepthToWater { get; set; }
        public double? PSI { get; set; }
        public double? StaticWaterLevel { get; set; }
        public string WeatherConditions { get; set; }
        public string SamplePeriod { get; set; } // 1s, 1m, 1h, 1d, etc.
        public string SampleTempUnit { get; set; } //C / F
        public string FieldActivityType { get; set; }
        public string DeployTime { get; set; }
        
    }
}