using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Transactions;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using services.Models;
using services.Models.Data;
using services.Resources;

//kb 5/1/2014
// this is a partial, so basically just a nice logical way of adding more to our DataActionController...

namespace services.Controllers
{
    public partial class DataActionController : ApiController
    {


        [HttpGet]
        public IEnumerable<LocationType> GetLocationTypes()
        {
            var db = ServicesContext.Current;
            return db.LocationType.AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<Instrument> GetAllInstruments()
        {
            var db = ServicesContext.Current;
            return db.Instruments.OrderBy(o => o.Name).ThenBy(o => o.SerialNumber).AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<WaterBody> GetWaterBodies()
        {
            var db = ServicesContext.Current;
            return db.WaterBodies.OrderBy(o => o.Name).AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<Source> GetSources()
        {
            var db = ServicesContext.Current;
            return db.Sources.AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<Instrument> GetInstruments()
        {
            var db = ServicesContext.Current;
            return db.Instruments.AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<InstrumentType> GetInstrumentTypes()
        {
            var db = ServicesContext.Current;
            return db.InstrumentType.AsEnumerable();
        }



        [HttpGet]
        public IEnumerable<Datastore> GetAllDatastores()
        {
            var db = ServicesContext.Current;
            return db.Datastores.AsEnumerable();
        }


        [HttpGet]
        public IEnumerable<Field> GetAllFields(int Id)
        {
            var db = ServicesContext.Current;
            return db.Fields.Where(o => o.FieldCategoryId == Id).OrderBy(o => o.Name).AsEnumerable();
        }


        [HttpGet]
        public Datastore GetDatastore(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new Exception("Configuration error: Datastore not recognized.");

            return datastore;
        }

        

        [HttpGet]
        public IEnumerable<Location> GetAllPossibleDatastoreLocations(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();
            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new Exception("Configuration error: Datastore not recognized");

            return datastore.Locations;

        }


        [HttpGet]
        public IEnumerable<Field> GetAllDatastoreFields(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new Exception("Configuration error: Datastore not recognized");

            return datastore.Fields;

        }


        [HttpGet]
        public IEnumerable<Project> GetDatastoreProjects(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new Exception("Configuration error: Datastore not recognized");

            return datastore.Projects;

        }

        [HttpGet]
        public IEnumerable<Dataset> GetDatastoreDatasets(int Id)
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();

            var datastore = db.Datastores.Find(Id);
            if (datastore == null)
                throw new Exception("Configuration error: Datastore not recognized");

            return datastore.Datasets;

        }


        
        [HttpPost]
        public HttpResponseMessage SaveDatasetField(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            DatasetField df = db.DatasetFields.Find(json.Id.ToObject<int>());

            if (df == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            df.Label = json.Label;
            df.Validation = json.Validation;
            df.Rule = json.Rule;
            df.FieldRoleId = json.FieldRoleId.ToObject<int>();
            try
            {
                df.OrderIndex = json.OrderIndex.ToObject<int>();
            }catch(Exception e){
                logger.Debug("didn't have an orderindex.");
            }
            df.ControlType = json.ControlType;
            df.SourceId = json.SourceId.ToObject<int>();
            
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);


        }

        [HttpPost]
        public HttpResponseMessage SaveProjectLocation(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new Exception("Configuration error.  Please try again.");

            Location location = json.Location.ToObject<Location>();

            //IF the incoming location has an ID then we update, otherwise we create a new project location
            if (location.Id == 0)
            {
                project.Locations.Add(location);
                db.SaveChanges();
                logger.Debug("success adding NEW proejct location!");
            }
            else
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug("success updating EXISTING project location!");
            }
            

            return new HttpResponseMessage(HttpStatusCode.OK);
            
        }

        [HttpPost]
        public HttpResponseMessage SaveInstrumentAccuracyCheck(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();

            Instrument instrument = db.Instruments.Find(json.InstrumentId.ToObject<int>());

            if (instrument == null)
                throw new Exception("Configuration error.  Please try again.");

            InstrumentAccuracyCheck ac = json.AccuracyCheck.ToObject<InstrumentAccuracyCheck>();

            ac.UserId = me.Id;

            instrument.AccuracyChecks.Add(ac);
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage SaveProjectInstrument(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            
            Instrument instrument = db.Instruments.Find(json.Instrument.Id.ToObject<int>());

            if (project == null || instrument == null)
                throw new Exception("Configuration error.  Please try again.");

            project.Instruments.Add(instrument);
            db.SaveChanges();
            logger.Debug("success adding NEW proejct instrument!");
            

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage RemoveProjectInstrument(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());
            Instrument instrument = db.Instruments.Find(json.InstrumentId.ToObject<int>());
            if (p == null || instrument == null)
                throw new Exception("Configuration error.  Please try again.");

            p.Instruments.Remove(instrument);
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage SaveInstrument(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project p = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (p == null)
                throw new Exception("Configuration error.  Please try again.");

            Instrument instrument = json.Instrument.ToObject<Instrument>();

            //if there is an instrument id already set, then we'll just update the instrument and call it good.
            //  otherwise we'll create the new instrument and a relationship to the project.

            if (instrument.Id == 0)
            {
                instrument.UserId = me.Id;
                instrument.OwningDepartmentId = Department.DEFAULT_DEPARTMENT;
                p.Instruments.Add(instrument);
                logger.Debug("created new instrument");
            }
            else
            {
                db.Entry(instrument).State = EntityState.Modified;
                logger.Debug("updated existing instrument");
            }
            
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        public HttpResponseMessage AddMasterFieldToDataset(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            int DatasetId = json.DatasetId.ToObject<int>();
            var dataset = db.Datasets.Find(DatasetId);

            int FieldId = json.FieldId.ToObject<int>();
            var field = db.Fields.Find(FieldId);

            DatasetField df = new DatasetField();

            df.DatasetId = dataset.Id;
            df.FieldId = field.Id;
            df.FieldRoleId = FieldRole.DETAIL;
            df.CreateDateTime = DateTime.Now;
            df.Label = field.Name;
            df.DbColumnName = field.DbColumnName;
            df.SourceId = 1;
            df.ControlType = field.ControlType;

            db.DatasetFields.Add(df);
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DeleteDatasetField(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            int DatasetId = json.DatasetId.ToObject<int>();
            var dataset = db.Datasets.Find(DatasetId);

            int FieldId = json.FieldId.ToObject<int>();
            var field = db.DatasetFields.Find(FieldId);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "DELETE FROM DatasetFields where DatasetId = " + dataset.Id + " and FieldId = " + field.Id;
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }

            }            

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        public HttpResponseMessage SaveMasterField(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            Field df = db.Fields.Find(json.Id.ToObject<int>());

            if (df == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            
            
            df.Name = json.Name;
            df.Validation = json.Validation;
            df.Rule = json.Rule;
            df.Units = json.Units;
            df.TechnicalName = json.TechnicalName;
            df.DbColumnName = json.DbColumnName;
            df.DataType = json.DataType;
            df.ControlType = json.ControlType;
            df.PossibleValues = json.PossibleValues;
            df.Description = json.Description;

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);


        }

    }
}
