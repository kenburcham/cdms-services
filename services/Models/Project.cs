using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using NLog;
using services.Resources;

namespace services.Models
{
    public class Project
    {
        
        private static Logger logger = LogManager.GetCurrentClassLogger(); 


        public int Id { get; set; }
        
        public int ProjectTypeId { get; set; }
        public int? OrganizationId { get; set; }
        public int? OwnerId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ProjectType ProjectType { get; set; }
        public virtual List<Location> Locations { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual List<File> Files { get; set; }
        public virtual User Owner { get; set; }

        [InverseProperty("ProjectEditor")]
        public virtual List<User> Editors { get; set; }

        [InverseProperty("ProjectInstruments")]
        public virtual List<Instrument> Instruments { get; set; }


        /**
         * Set with list of metadata values populated with: MetadataPropertyId, Values, UserId 
         * and we will save changed or new values with a new effdt.
         */
        [NotMapped]
        public List<MetadataValue> Metadata { 
            set
            {
                MetadataHelper.saveMetadata(this.Metadata, value, this.Id);
            }
            get
            {
                return MetadataHelper.getMetadata(this.Id, MetadataEntity.ENTITYTYPE_PROJECT);
            }
            
        }

        /**
         * Deletes all metadata for this project
         */ 
        public void deleteMetadata()
        {
            MetadataHelper.deleteMetadata(this.Id, MetadataEntity.ENTITYTYPE_PROJECT);
        }


        /**
         * Removes any related objects that aren't configured for 
         * referential integrity.
         */ 
        internal void deleteRelatedData()
        {
            var db = ServicesContext.Current;
            //first, let's find and remove any files.

            ICollection<File> file_query = (from f in Files
                                                    where f.ProjectId == this.Id
                                                    select f).ToList();

            foreach (File file in file_query)
            {
                db.Files.Remove(file);
            }

            this.deleteMetadata();
        }
    }
}