using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class AdultWeir_Header : DataHeader
    {

        public double? AirTemperature { get; set; }
        public double? AirTemperatureF { get; set; }
        public double? WaterTemperature { get; set; }
        public double? WaterTemperatureF { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string Technicians { get; set; }
        public double? WaterFlow { get; set; }
        public string Comments { get; set; }
        public string CollectionType { get; set; }        
    }
}