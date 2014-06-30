using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models
{
    public class Instrument
    {
        public const int ACTIVE_STATUSID = 0; //active
        public const int INACTIVE_STATUSID = 1; //inactive


        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int? PurchasingProgramProjectId { get; set; }
        public int OwningDepartmentId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? EnteredService { get; set; }
        public DateTime? EndedService { get; set; }
        public int InstrumentTypeId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        
        public virtual Department OwningDepartment { get; set; }
        public virtual InstrumentType InstrumentType { get; set; }

        public virtual List<InstrumentAccuracyCheck> AccuracyChecks {  get; set; }


        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        [InverseProperty("Instruments")]
        public virtual List<Project> ProjectInstruments { get; set; }

        public Instrument()
        {
            CreateDateTime = DateTime.Now;
            StatusId = ACTIVE_STATUSID;
        }
    }

    public class InstrumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class InstrumentAccuracyCheck
    {
        public const string DEFAULT_BATH1GRADE = "C"; //uncalibrated
        public const int DEFAULT_CHECKMETHOD = 5; //unknown

        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime CheckDate { get; set; }
        public int CheckMethod { get; set; } //list is enumerated as a metadata field
        public string Bath1Grade { get; set; }
        public string Bath2Grade { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual Instrument Instrument { get; set; }
        
        [JsonIgnore]
        public virtual User User { get; set; }

        public InstrumentAccuracyCheck()
        {
            CreateDateTime = DateTime.Now;
            Bath1Grade = DEFAULT_BATH1GRADE;
            CheckMethod = DEFAULT_CHECKMETHOD;
        }

    }
}