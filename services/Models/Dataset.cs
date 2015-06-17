using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using NLog;
using services.Resources;

namespace services.Models
{
    public class Dataset
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public const int STATUS_ACTIVE = 1;
        public const int STATUS_INACTIVE = 0;
        public const int STATUS_DELETED = 2;

        public const int ACCESS_PUBLIC = 1;
        public const int ACCESS_PRIVATE = 2;
        public const int ACCESS_DEPARTMENT = 3; 

        public int Id { get; set; }
        public int ProjectId { get; set; }          //what project is this dataset related to?
        public int DefaultRowQAStatusId { get; set; }
        public int DefaultActivityQAStatusId { get; set; }
        public int StatusId { get; set; } //active, inactive, deleted
        public int? DatastoreId { get; set; }
        public string Config { get; set; } //config for this dataset...

        public DateTime CreateDateTime { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        
        //collection of QAStatuses - which statuses are possible for this dataset?
        public virtual QAStatus DefaultRowQAStatus { get; set; }

        public virtual List<DatasetField> Fields { get; set; }
        public virtual Datastore Datastore { get; set; }

        [InverseProperty("Datasets")]
        public virtual List<QAStatus> QAStatuses { get; set; } //dataset activity qa options

        [InverseProperty("QARowDatasets")]
        public virtual List<QAStatus> RowQAStatuses { get; set; } //dataset row qa options

        [NotMapped]
        public List<MetadataValue> Metadata
        {
            set
            {
                MetadataHelper.saveMetadata(this.Metadata, value, this.Id);
            }
            get
            {
                return MetadataHelper.getMetadata(this.Id, MetadataEntity.ENTITYTYPE_DATASET);
            }

        }

        /**
         * Deletes all metadata for this dataset
         */
        public void deleteMetadata()
        {
            MetadataHelper.deleteMetadata(this.Id, MetadataEntity.ENTITYTYPE_DATASET);
        }

        //TODO: really want to cache this somehow...
        //note: this is specific to datasetfields, so we can't use it for the global query.
        internal string getExportSelectString()
        {
            var header_fields = string.Join(",", this.Fields.Where(o => o.FieldRoleId == FieldRole.HEADER).OrderBy(o => o.Label).Select(o => o.DbColumnName));
            header_fields += (header_fields == "") ? "" : ","; //add on the ending comma if applicable
            var detail_fields = string.Join(",",this.Fields.Where(o => o.FieldRoleId == FieldRole.DETAIL).OrderBy(o => o.Label).Select(o => o.DbColumnName)) + ",";

            //add on any "system" fields we want to also return
            var activity_fields = "ActivityDate,";
            var system_fields = "CreateDate,QAStatusId,QAStatusName, ActivityQAComments, LocationId,ActivityQAStatusId,DatasetId,ActivityId,RowId, RowStatusId";

            return activity_fields + header_fields + detail_fields + system_fields;
        }

        internal IEnumerable<string> getExportLabelsList()
        {
            IEnumerable<string> labels = null;
            try
            {
                labels = new string[] { "ActivityDate" };

                labels = labels
                  .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.HEADER).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                  .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.DETAIL).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                  .Concat(new List<string>(new string[] { "CreateDate", "QAStatusId", "QAStatus", "ActivityQAComments", "LocationId", "ActivityQAStatusId", "DatasetId", "ActivityId","RowId","RowStatusId"}));

                foreach (var item in labels)
                {
                    logger.Debug(item);    
                }
                
            }
            catch (Exception e)
            {
                logger.Debug("Error!");
                logger.Debug(e);
            }
            return labels;

        }

      }

    

}