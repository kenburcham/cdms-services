using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using services.Models;

namespace services.Models.Data
{
    public class CreelSurvey_Header: DataHeader
    {
        public string SurveyType { get; set; }
        public string Species { get; set; }
        public string Shift { get; set; }
        public string Season { get; set; }
        public string Surveyor { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? NumberAnglersObserved { get; set; }
        public int? NumberAnglersInterviewed { get; set; }
        public string Comments { get; set; }
        public string FieldSheetFile { get; set; }
//waterbody is via location
    }
}