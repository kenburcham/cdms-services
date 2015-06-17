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
    [System.Web.Http.Authorize]
    public partial class DataActionController : ApiController
    {

        [HttpGet]
        public IEnumerable<TimeZoneInfo> GetTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();           
        }

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
            //return db.Instruments.OrderBy(o => o.Name).ThenBy(o => o.SerialNumber).AsEnumerable();
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
        public HttpResponseMessage UpdateFile(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new Exception("Configuration error.  Please try again.");

            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            services.Models.File in_file = json.File.ToObject<services.Models.File>();

            services.Models.File existing_file = project.Files.Where(o => o.Id == in_file.Id).SingleOrDefault();

            if (existing_file == null)
                throw new Exception("File not found.");

            existing_file.Title = in_file.Title;
            existing_file.Description = in_file.Description;
            db.Entry(existing_file).State = EntityState.Modified;
            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DeleteFile(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new Exception("Configuration error.  Please try again.");

            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            services.Models.File in_file = json.File.ToObject<services.Models.File>();

            services.Models.File existing_file = project.Files.Where(o => o.Id == in_file.Id).SingleOrDefault();

            if (existing_file == null)
                throw new Exception("File not found.");

            project.Files.Remove(existing_file);
            db.Entry(project).State = EntityState.Modified;

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

            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            Location location = json.Location.ToObject<Location>();
            location.UserId = me.Id;

            //IF the incoming location has an ID then we update, otherwise we create a new project location
            if (location.Id == 0)
            {
                location.CreateDateTime = DateTime.Now;
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

            string result = JsonConvert.SerializeObject(location);

            //TODO: actual error/success message handling
            //string result = "{\"message\": \"Success\"}";

            HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

            return resp;

            //return new HttpResponseMessage(HttpStatusCode.OK);
            
        }

        [HttpPost]
        public HttpResponseMessage DeleteLocation(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();

            Location loc = db.Location.Find(json.LocationId.ToObject<int>());

            if (loc == null)
                throw new Exception("Configuration error.");

            if (db.Activities.Where(o => o.LocationId == loc.Id).Count() == 0)
            {
                db.Location.Remove(loc);
                db.SaveChanges();
                logger.Debug("Deleted location "+loc.Id+" because there was no activity.");
            }
            else
            {
                logger.Debug("Tried to delete location " + loc.Id + " when activities exist.");
                throw new Exception("Location Delete failed because activities exist!");
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

            if (ac.Id == 0)
            {
                instrument.AccuracyChecks.Add(ac);
                db.SaveChanges();
            }
            else
            {
                db.Entry(ac).State = EntityState.Modified;
                db.SaveChanges();
            }

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage SaveProjectInstrument(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;
            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());

            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

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

            if (!p.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

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

            if (!p.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            Instrument instrument = json.Instrument.ToObject<Instrument>();
            instrument.OwningDepartmentId = json.Instrument.OwningDepartmentId.ToObject<int>();

            logger.Debug("The id == " + instrument.OwningDepartmentId);

            //if there is an instrument id already set, then we'll just update the instrument and call it good.
            //  otherwise we'll create the new instrument and a relationship to the project.

            if (instrument.Id == 0)
            {
                instrument.UserId = me.Id;
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
            if (dataset == null)
                throw new Exception("Dataset could not be found: " + DatasetId);

            int FieldId = json.FieldId.ToObject<int>();
            var field = db.DatasetFields.Find(FieldId);
            if (field == null)
                throw new Exception("Field could not be retrieved for dataset: " + DatasetId);

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
