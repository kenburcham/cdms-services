using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using services.Resources;

namespace services.Models
{
    public class Field
    {
        private string _possibleValues;

        public int Id { get; set; }
        public int FieldCategoryId { get; set; }
        public string TechnicalName { get; set; }
        public string DbColumnName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Units { get; set; }
        public string PossibleValues
        {
            get
            {
                if (this.ControlType == "lookup" || this.ControlType == "multilookup")
                {
                    return LookupFieldHelper.getPossibleValues(_possibleValues);
                }
                else
                    return _possibleValues;
            }

            set
            {
                _possibleValues = value;
            }
        }
        public string Validation { get; set; }
        public string Rule { get; set; }
        public string DataType { get; set; }
        public string ControlType { get; set; }

        public virtual FieldCategory FieldCategory { get; set; }

    }

    public class FieldCategory {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    //FieldRole == Header, Detail, Summary, Statistic, ...
    public class FieldRole {

        public const int HEADER = 1;
        public const int DETAIL = 2;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }


}