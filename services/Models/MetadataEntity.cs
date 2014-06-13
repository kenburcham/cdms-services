using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models
{
    // Type of metadata / category / entity.  like:
    //FieldConfigurations
    //Datasets
    //Projects

    public class MetadataEntity
    {
        public const int ENTITYTYPE_PROJECT = 1; //Projects
        public const int ENTITYTYPE_HABITAT = 2; 
        public const int ENTITYTYPE_DATASET = 5; //Datasets

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }



    }
}