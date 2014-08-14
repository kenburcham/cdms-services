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

namespace services.Controllers
{
    [Authorize]
    public partial class DataActionController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public DataTable GetHeadersDataForDataset(int id)
        {
            var db = ServicesContext.Current;
            Dataset dataset = db.Datasets.Find(id);
            if (dataset == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            string query = "SELECT h.* FROM " + dataset.Datastore.TablePrefix + "_Header_VW h JOIN Activities a on a.Id = h.ActivityId WHERE a.DatasetId = " + dataset.Id;

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }


        [HttpPost]
        public IEnumerable<MetadataValue> GetMetadataFor(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();
            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            int EntityTypeId = json.EntityTypeId.ToObject<int>();

            if (project == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            return MetadataHelper.getMetadata(project.Id, EntityTypeId).AsEnumerable();

        }

        [HttpGet]
        public IEnumerable<Dataset> GetMyDatasets()
        {
            var db = ServicesContext.Current;
            User me = AuthorizationManager.getCurrentUser();
            var mydatasets = "";
            try
            {
                mydatasets = me.UserPreferences.Where(o => o.Name == UserPreference.DATASETS).FirstOrDefault().Value;
            }
            catch (Exception e)
            {
                logger.Debug("Couldn't get your datasets -- probably don't have any favorites.");
                logger.Debug(e);
            }

            var datasets = db.Datasets.SqlQuery("SELECT * FROM Datasets WHERE Id in (" + mydatasets +")");

            return datasets;
        }

        [HttpPost]
        public HttpResponseMessage SetQaStatus(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();
            Activity activity = db.Activities.Find(json.ActivityId.ToObject<int>());

            if (activity == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            logger.Debug("Userid = " + me.Id + " Activity = " + activity.Id);

            //TODO: verify that the user is authorized to modify this activity?  -- yes, that would be nice. (or add Authorized annotation) TODO!


            var aq = new ActivityQA();
            aq.ActivityId = activity.Id;
            aq.EffDt = DateTime.Now;
            aq.Comments = json.Comments;
            aq.UserId = me.Id; //current user.
            aq.QAStatusId = json.QAStatusId;
            
            db.ActivityQAs.Add(aq);

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);


        }

        [HttpPost]
        public HttpResponseMessage SetProjectEditors(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            Project project = db.Projects.Find(json.ProjectId.ToObject<int>());
            if (project == null)
                throw new Exception("Configuration error.");

            User me = AuthorizationManager.getCurrentUser(); 
            if (me == null)
                throw new Exception("Configuration error.");

            //verify that the sender is the project owner. 
            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            //First -- remove all editors from this project.
            project.Editors.RemoveAll(o => o.Id > 0);
            db.SaveChanges();

            foreach (var item in json.Editors)
            {
                User user = db.User.Find(item.Id.ToObject<int>());
                if (user == null)
                    logger.Debug("Wow -- user not found!: " + item.Id);
                else
                {
                    logger.Debug("Adding: " + item.Id);
                    project.Editors.Add(user);
                }
            }

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage SetDatasetMetadata(JObject jsonData)
        {
            var db = ServicesContext.Current;
            dynamic json = jsonData;

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null)
                throw new Exception("Configuration error.");

            Project project = db.Projects.Find(dataset.ProjectId);
            
            User me = AuthorizationManager.getCurrentUser();
            if (!project.isOwnerOrEditor(me))
                throw new Exception("Configuration error.");

            //Now save metadata
            List<MetadataValue> metadata = new List<MetadataValue>();

            foreach (var jmv in json.Metadata)
            {
                var mv = jmv.ToObject<MetadataValue>();
                mv.UserId = me.Id;
                metadata.Add(mv);
                logger.Debug("Found new metadata: " + mv.MetadataPropertyId + " + + " + mv.Values);
            }

            //fire setMetdata which will handle persisting the metadata
            dataset.Metadata = metadata;

            db.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpPost]
        public HttpResponseMessage DeleteDatasetActivities(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());

            Project project = db.Projects.Find(dataset.ProjectId);

            if (project == null)
                throw new Exception("Configuration error.  Please try again.");

            User me = AuthorizationManager.getCurrentUser();
            if (!project.isOwnerOrEditor(me))
                throw new Exception("Configuration error.");

            var Activities = new List<string>();

            foreach (var item in json.Activities)
            {
                Activities.Add(""+item.Id);
            }

            var ActivityIds = string.Join(",", Activities);
            var DataTable = dataset.Datastore.TablePrefix; 

            //open a raw database connection...
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                con.Open();

                var query = "DELETE FROM " + DataTable + "_Detail WHERE ActivityId in ("+  ActivityIds + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }

                query = "DELETE FROM " + DataTable + "_Header WHERE ActivityId in (" + ActivityIds + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }

                query = "DELETE FROM ActivityQAs WHERE ActivityId in (" + ActivityIds + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }

                query = "DELETE FROM Activities WHERE DatasetId = " + dataset.Id + " AND Id in (" + ActivityIds + ")";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    logger.Debug(query);
                    cmd.ExecuteNonQuery();
                }
            }

            
            return new HttpResponseMessage(HttpStatusCode.OK);

        }



        //QUERY
        //QueryDatasetActivities -- can call with a datasetId or a datastoreId
        [HttpPost]
        public DataTable QueryDatasetActivities(JObject jsonData)
        {
            var db = ServicesContext.Current;
            DataTable datatable = null;

            dynamic json = jsonData;
            
            //let's see if we're dealing with a dataset 
            if (json["DatasetId"] is JToken)
            {
                //grab a reference to this dataset so we can parse incoming fields
                Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
                if (dataset == null)
                    throw new Exception("Configuration error. Please try again.");

                logger.Debug("Alright!  we are working with dataset: " + dataset.Id);

                datatable = getQueryResults(dataset, json);

            }


            //let's see if we're dealing with a datastore.
            if (json["DatastoreId"] is JToken)
            {
                //grab a reference to this dataset so we can parse incoming fields
                Datastore datastore = db.Datastores.Find(json.DatastoreId.ToObject<int>());
                if (datastore == null)
                    throw new Exception("Configuration error. Please try again.");

                logger.Debug("Alright!  we are working with datastore: " + datastore.Id);

                datatable = getQueryResults(datastore, json);

            }

            
            return datatable;
            
        }

        //Writes csv export file out to a file and returns the url.
        [HttpPost]
        public ExportResult DownloadDatasetActivities(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            //grab a reference to this dataset so we can parse incoming fields
            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            logger.Debug("Alright!  we are working with dataset: " + dataset.Id);

            DataTable dt = getQueryResults(dataset, json);

            logger.Debug("Download data -- we have a result back.");

            string Filename = json.Filename;
            Filename = Filename.Replace("\"", string.Empty);
            Filename = Filename.Replace("\\", string.Empty);
            Filename = Filename.Replace("/", string.Empty);
            
            logger.Debug("Incomfing filename specified: " + Filename);

            string root = System.Web.HttpContext.Current.Server.MapPath("~/exports");
            string the_file = root + @"\" + dataset.Id + @"_" + me.Id + @"\" + Filename;

            logger.Debug("saving file to location: " + the_file);

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(the_file)); //will create if necessary.

            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            rootUrl += "/servicesSTAGE/exports/" + dataset.Id + "_" + me.Id + "/" + Filename;

            using (TextWriter writer = System.IO.File.CreateText(the_file)) //will overwrite = good
            {
                using (var csv = new CsvWriter(writer))
                {
                    IEnumerable<string> columnNames = dataset.getExportLabelsList();//dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

                    //columns
                    foreach (var header in columnNames)
                    {
                        csv.WriteField(header);
                    }
                    csv.NextRecord();

                    //fields
                    foreach (DataRow row in dt.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        foreach (var field in fields)
                        {
                            //replace out the multiselect array stuff.
                            var f = field.Replace("[]", string.Empty).Replace("[\"", string.Empty).Replace("\"]", string.Empty).Replace("\",\"", ",");
                            csv.WriteField(f);
                        }
                        csv.NextRecord();
                    }
                }
            }

            //TODO-- error handling?

            ExportResult result = new ExportResult();
            result.success = true;
            result.file = rootUrl;
            result.errors = null;
            
            return result;

        }


        [HttpPost]
        public HttpResponseMessage UpdateDatasetActivities(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null)
                throw new Exception("Configuration Error.");

            Project project = db.Projects.Find(dataset.ProjectId);
            if (project == null)
                throw new Exception("Configuration Error");

            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            //setup our generic data stuff
            var data_header_name = dataset.Datastore.TablePrefix + "_Header";
            var data_detail_name = dataset.Datastore.TablePrefix + "_Detail";
            var dbset_header = db.GetDbSet(data_header_name);
            var dbset_detail = db.GetDbSet(data_detail_name);
            var dbset_header_type = db.GetTypeFor(data_header_name);
            var dbset_detail_type = db.GetTypeFor(data_detail_name);

            foreach (var item in json.activities)
            {
                
                if (item is JProperty)
                {
                    var prop = item as JProperty;
                    dynamic activity_json = prop.Value;

                    logger.Debug("Updating activity id: " + json.ActivityId);

                    Activity activity = db.Activities.Find(json.ActivityId.ToObject<int>());
                    if (activity == null)
                    {
                        throw new Exception("Invalid Activity.");
                    }

                    activity.LocationId = activity_json.LocationId;
                    try
                    {
                        activity.ActivityDate = activity_json.ActivityDate;
                    }
                    catch (Exception e)
                    {
                        logger.Debug("Ooops had an error converting date: " + activity_json.ActivityDate);
                        logger.Debug(e.ToString());
                        throw (e);
                    }

                    //activity.DatasetId = json.DatasetId;
                    activity.UserId = me.Id;
                    activity.SourceId = 1;                                          // TODO get from data
                    activity.ActivityTypeId = 1;
                    activity.InstrumentId = activity_json.InstrumentId;
                    activity.AccuracyCheckId = activity_json.AccuracyCheckId;
                    activity.PostAccuracyCheckId = activity_json.PostAccuracyCheckId;
                    activity.Timezone = activity_json.Timezone;

                    //activity.CreateDate = DateTime.Now;

                    db.Entry(activity).State = EntityState.Modified;
                    db.SaveChanges();

                    //updated the activity
                    
                    logger.Debug("Updated an activity: ");
                    logger.Debug(" LocationID = " + activity_json.LocationId);
                    logger.Debug(" ActivityDate = " + activity_json.ActivityDate);
                    logger.Debug("  ID = " + activity.Id);

                    //now check our activity status -- update it if we've changed.
                    //if(activity.ActivityQAStatus.ActivityId != )

                    dynamic activityqastatus = activity_json.ActivityQAStatus;

                    //logger.Debug(activity_json.ActivityQAStatus);
                    
                    //logger.Debug(activityqastatus.QAStatusId.ToObject<int>());

                    ActivityQA newQA = new ActivityQA();
                    newQA.ActivityId = activity.Id;
                    newQA.QAStatusId = activityqastatus.QAStatusID.ToObject<int>();
                    newQA.Comments = activityqastatus.Comments;
                    newQA.EffDt = DateTime.Now;
                    newQA.UserId = activity.UserId;

                    db.ActivityQAs.Add(newQA);
                    db.SaveChanges();


                    //get our last header and then check against incoming header field values to see if anything has changed.
                    var last_header_list = dbset_header.SqlQuery("SELECT * FROM " + data_header_name + " WHERE ActivityId = " + activity.Id + " ORDER BY EffDt DESC");
                        //.SqlQuery("SELECT * FROM " + data_header_name + " WHERE ActivityId = " + activity.Id + " ORDER BY EffDt DESC").AsQueryable().f; 
                        //db.AdultWeir_Header.Where(o => o.ActivityId == activity.Id).OrderByDescending(d => d.EffDt).FirstOrDefault();

                    var last_header = this.getFirstItem(last_header_list);

                    logger.Debug("Ok -- here we are with our lastheader:");
                    logger.Debug(last_header);


                    if (last_header == null)
                        throw new Exception("Somehow there is no previous header even though we are trying to update.");

                    bool header_updated = false;

                    //spin through and check the header fields for changes...
                    foreach (JProperty header_field in activity_json.Header)
                    {
                        logger.Debug("Checking last header value of field : '" + header_field.Name + "' with incoming value + '" + header_field.Value+"'");
                        var objval = last_header.GetType().GetProperty(header_field.Name).GetValue(last_header, null);
                        if (objval != null)
                        {
                            logger.Debug("Checking last header value of field : " + header_field.Name + " with value " + objval + " + with incoming value + " + header_field.Value);

                            if (objval.ToString() != header_field.Value.ToString())
                            {
                                logger.Debug("a different value! we'll save a header then...");
                                header_updated = true;
                                break;
                            }
                        }
                        else
                        {
                            if (header_field.Value.ToString() != "")
                            {
                                logger.Debug("Dunno why, but objval was null." + header_field + " we are going to save a new one.");
                                header_updated = true;
                                break;
                            }
                        }
                    }

                    if (header_updated)
                    {
                        logger.Debug("Saving a new header then");
                        var header = activity_json.Header.ToObject(dbset_header_type);

                        //now do the saving! -- this works the exact same way for update as it does for new
                        header.ActivityId = activity.Id;
                        header.ByUserId = activity.UserId;
                        header.EffDt = DateTime.Now;
                        dbset_header.Add(header);
                        db.SaveChanges();
                    }

                    //there are three possible cases of changes:
                    //  1) updated row (has ID and is in "updatedRowIds" list)
                    //  2) new row (has no ID)
                    //  3) deleted row (is not in the list, ID is in "deletedRowIds" list)

                    //we ALWAYS make such indication by INSERTING a new row with a matching rowid + activityid + new current effective date.
                    //  exception is NEW row which gets and incremented rowid

                    //first, lets lookup our last row id so we have a place to start if we add rows.                    
                    int rowid = 1;

                    var last_row_list = dbset_detail.SqlQuery("SELECT * FROM " + data_detail_name + " WHERE ActivityId = "+ activity.Id + " AND RowStatusId = " + DataDetail.ROWSTATUS_ACTIVE + " ORDER BY RowId DESC");
                        //db.AdultWeir_Detail.Where(o => o.ActivityId == activity.Id).Where(o => o.RowStatusId == DataDetail.ROWSTATUS_ACTIVE).OrderByDescending(d => d.RowId).FirstOrDefault();
                    
                    var last_row = this.getFirstItem(last_row_list);
                    if (last_row != null)
                    {
                        rowid = last_row.RowId+1;
                    }
                    else
                        logger.Debug("Hmm there were no previous details rows for activity : " + activity.Id + " so we are starting at 1.");

                    //now lets iterate our incoming rows and see what we've got.
                    var details = new List<DataDetail>();

                    List<int> updated_rows = new List<int>();
                    foreach (var updated_row in json.updatedRowIds)
                    {
                        logger.Debug("Found an updated row: " + updated_row);
                        updated_rows.Add(updated_row.ToObject<int>());
                    }

                    List<int> deleted_rows = new List<int>();
                    foreach(var deleted_row in json.deletedRowIds)
                    {
                        logger.Debug("Found a deleted row: " + deleted_row);
                        deleted_rows.Add(deleted_row.ToObject<int>());
                        if (updated_rows.Contains(deleted_row.ToObject<int>()))
                            updated_rows.Remove(deleted_row.ToObject<int>());
                    }


                    foreach (var detailitem in activity_json.Details)
                    {
                        var adw = detailitem.ToObject(dbset_detail_type);

                        logger.Debug("spinning through incoming details: " + adw.Id);

                        if (adw.Id == 0)
                        {
                            //new record
                            adw.RowId = rowid; rowid++;
                            details.Add(adw);
                        }
                        else
                        {
                            //deleted or updated?
                            if (updated_rows.Contains(adw.Id))
                            {
                                //updated
                                adw.Id = 0;
                                details.Add(adw);
                            }
                            else if (deleted_rows.Contains(adw.Id))
                            {
                                //deleted
                                adw.Id = 0;
                                adw.RowStatusId = DataDetail.ROWSTATUS_DELETED;
                                details.Add(adw);
                            }
                            //otherwise nothing.
                        }

                    }



                    foreach (var detail in details)
                    {
                        detail.ActivityId = activity.Id;
                        detail.ByUserId = activity.UserId;
                        detail.EffDt = DateTime.Now;
                        
                        //TODO: activity QA ID + comment

                        dbset_detail.Add(detail);

                    }

                    db.SaveChanges();

                    //If there is a ReadingDateTime field in use, set the activity description to be the range of reading dates for this activity.
                    if (dataset.Datastore.TablePrefix == "WaterTemp") // others with readingdatetime?
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
                        {
                            con.Open();
                            var query = "update Activities set Description = (select concat(convert(varchar,min(ReadingDateTime),111), ' - ', convert(varchar,max(ReadingDateTime),111)) from " + dataset.Datastore.TablePrefix + "_Detail_VW where ActivityId = " + activity.Id + ") where Id = " + activity.Id;

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                logger.Debug(query);
                                cmd.ExecuteNonQuery();
                            }

                        }
                    }                        
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private dynamic getFirstItem(dynamic list)
        {
            dynamic first = null;
            foreach (var item in list)
            {
                first = item;
                break;
            }
            return first;
        }

        [HttpPost]
        public HttpResponseMessage SaveDatasetActivities(JObject jsonData)
        {
            logger.Debug("Saving dataset activities: ");
            var db = ServicesContext.Current;

            User me = AuthorizationManager.getCurrentUser();

            dynamic json = jsonData;

            //COPY PASTE -- TODO -- reduce code smell!
            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null)
                throw new Exception("Configuration Error.");

            Project project = db.Projects.Find(dataset.ProjectId);
            if (!project.isOwnerOrEditor(me))
                throw new Exception("Authorization error.");

            //setup our generic data stuff
            var data_header_name = dataset.Datastore.TablePrefix + "_Header";
            var data_detail_name = dataset.Datastore.TablePrefix + "_Detail";
            var dbset_header = db.GetDbSet(data_header_name);
            var dbset_detail = db.GetDbSet(data_detail_name);
            var dbset_header_type = db.GetTypeFor(data_header_name);
            var dbset_detail_type = db.GetTypeFor(data_detail_name);

            //var duplicateActivities = new List<Activity>();

            var new_records = new List<Activity>();

            //wrap this in a transaction
            
            
                foreach (var item in json.activities)
                {
                    var scope = new TransactionScope(
                    TransactionScopeOption.RequiresNew,
                    new TransactionOptions()
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    });

                    int newActivityId = 0 ;

                    using (scope)
                    {

                        if (item is JProperty)
                        {
                            var prop = item as JProperty;
                            dynamic activity_json = prop.Value;

                            Activity activity = new Activity();
                            activity.LocationId = activity_json.LocationId;
                            
                            try
                            {
                                activity.ActivityDate = activity_json.ActivityDate;
                            }
                            catch (Exception e)
                            {
                                //TODO -- this is a very bad idea if the date is wrong...
                                logger.Debug("Ooops had an error converting date: " + activity_json.ActivityDate);
                                logger.Debug(e.ToString());

                                throw e;

                            }

                            try
                            {

                                activity.DatasetId = json.DatasetId;
                                activity.UserId = me.Id;
                                activity.SourceId = 1;                                                                  // TODO 
                                activity.ActivityTypeId = 1;
                                activity.CreateDate = DateTime.Now;
                                activity.InstrumentId = activity_json.InstrumentId;
                                activity.AccuracyCheckId = activity_json.AccuracyCheckId;
                                activity.PostAccuracyCheckId = activity_json.PostAccuracyCheckId;
                                activity.Timezone = activity_json.Timezone;

                                logger.Debug("and we have finished parameters.");
                                /*
                                //check for duplicates.  If it is a duplicate, add it to our list and bail out.
                                if (activity.isDuplicate())
                                {
                                    duplicateActivities.Add(activity);
                                }
                                */

                                db.Activities.Add(activity);
                                db.SaveChanges();

                                dynamic activityqastatus = activity_json.ActivityQAStatus;

                                //logger.Debug(activity_json.ActivityQAStatus);

                                //logger.Debug(activityqastatus.QAStatusId.ToObject<int>());

                                newActivityId = activity.Id;

                                ActivityQA newQA = new ActivityQA();
                                newQA.ActivityId = activity.Id;
                                newQA.QAStatusId = activityqastatus.QAStatusID.ToObject<int>();
                                newQA.Comments = activityqastatus.Comments;
                                newQA.EffDt = DateTime.Now;
                                newQA.UserId = activity.UserId;

                                db.ActivityQAs.Add(newQA);
                                db.SaveChanges();


                                //                        logger.Debug("Created a new activity: ");
                                //                        logger.Debug(" LocationID = " + activity_json.LocationId);
                                //                        logger.Debug(" ActivityDate = " + activity_json.ActivityDate);
                                //                        logger.Debug("  ID = " + activity.Id);

                                var header = activity_json.Header.ToObject(dbset_header_type);
                                var details = new List<DataDetail>();

                                foreach (var detailitem in activity_json.Details)
                                {
                                    details.Add(detailitem.ToObject(dbset_detail_type));
                                }

                                //now do the saving!
                                header.ActivityId = activity.Id;
                                header.ByUserId = activity.UserId;
                                header.EffDt = DateTime.Now;
                                dbset_header.Add(header);

                                //details
                                int rowid = 1;
                                foreach (var detail in details)
                                {
                                    detail.RowId = rowid;
                                    detail.RowStatusId = DataDetail.ROWSTATUS_ACTIVE;
                                    detail.ActivityId = activity.Id;
                                    detail.ByUserId = activity.UserId;
                                    detail.EffDt = DateTime.Now;

                                    dbset_detail.Add(detail);
                                    rowid++;
                                }

                                db.SaveChanges();

                                scope.Complete(); //complete the transaction since nothing blew up!
                                
                                //logger.Debug(((JObject)JToken.FromObject(data)).ToString());
                                new_records.Add(activity);
                            }
                            catch (Exception e)
                            {
                                logger.Debug("An error occurred: ", e.ToString());
                                throw e; //rethrow so that it'll come back as an error in the client.
                            }

                        } //if
                    } //using

                    //If there is a ReadingDateTime field in use, set the activity description to be the range of reading dates for this activity.
                    if (newActivityId != 0 && dataset.Datastore.TablePrefix == "WaterTemp") // others with readingdatetime?
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
                        {
                            con.Open();
                            var query = "update Activities set Description = (select concat(convert(varchar,min(ReadingDateTime),111), ' - ', convert(varchar,max(ReadingDateTime),111)) from " + dataset.Datastore.TablePrefix + "_Detail_VW where ActivityId = " + newActivityId + ") where Id = " + newActivityId;

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                logger.Debug(query);
                                cmd.ExecuteNonQuery();
                            }

                        }
                    }
                } //foreach


            /*
            logger.Debug(duplicateActivities);

            var importResult = new ImportResult();
            importResult.duplicates = duplicateActivities;
            importResult.success = true;
            */
           
            
            
            //string result = JsonConvert.SerializeObject(new_records);
            
            //HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            //resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

            //return resp;
            
            return new HttpResponseMessage(HttpStatusCode.OK);
        }



        /**
       * Handle uploaded files
       * IEnumerable<File>
       */
        [HttpPost]
        public Task<HttpResponseMessage> UploadImportFile()
        {
            logger.Debug("starting to process incoming files.");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);

            logger.Debug("saving files to location: " + root);
            logger.Debug(" and the root url = " + rootUrl);

            var provider = new MultipartFormDataStreamProvider(root);

            User me = AuthorizationManager.getCurrentUser();

            var db = ServicesContext.Current;

            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(o =>
                {

                    if (o.IsFaulted || o.IsCanceled)
                    {
                        logger.Debug("Error: " + o.Exception.Message);
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, o.Exception));
                    }

                    //Look up our project
                    Int32 ProjectId = Convert.ToInt32(provider.FormData.Get("ProjectId"));
                    logger.Debug("And we think the projectid === " + ProjectId);

                    Project project = db.Projects.Find(ProjectId);
                    if (!project.isOwnerOrEditor(me))
                        throw new Exception("Authorization error.");


                    var newFileName = "";

                    foreach (MultipartFileData file in provider.FileData)
                    {

                        logger.Debug("Filename = " + file.LocalFileName);
                        logger.Debug("Orig = " + file.Headers.ContentDisposition.FileName);
                        logger.Debug("Name? = " + file.Headers.ContentDisposition.Name);

                        var fileIndex = ActionController.getFileIndex(file.Headers.ContentDisposition.Name); //"uploadedfile0" -> 0
                        var filename = file.Headers.ContentDisposition.FileName;
                        filename = filename.Replace("\"", string.Empty);

                        if (!String.IsNullOrEmpty(filename))
                        {
                            try
                            {
                                newFileName = ActionController.relocateProjectFile(
                                                file.LocalFileName,
                                                ProjectId,
                                                filename,
                                                true);

                                
                                /*
                                File newFile = new File();
                                newFile.Title = provider.FormData.Get("Title_" + fileIndex); //"Title_1, etc.
                                newFile.Description = provider.FormData.Get("Description_" + fileIndex); //"Description_1, etc.
                                newFile.Name = info.Name;//.Headers.ContentDisposition.FileName;
                                newFile.Link = rootUrl + "/services/uploads/" + ProjectId + "/" + info.Name; //file.LocalFileName;
                                newFile.Size = (info.Length / 1024).ToString(); //file.Headers.ContentLength.ToString();
                                newFile.FileTypeId = FileType.getFileTypeFromFilename(info);
                                newFile.UserId = me.Id;
                                logger.Debug(" Adding file " + newFile.Name + " at " + newFile.Link);

                                files.Add(newFile);
                                 */ 
                            }
                            catch (Exception e)
                            {
                                logger.Debug("Error: " + e.ToString());
                            }
                        }
                         
                    }

                    logger.Debug("Done saving files.");

                    ImportDataResult data = new ImportDataResult();
                    var info = new System.IO.FileInfo(newFileName);

                    //process the file and return all the data!

                    //TODO: refactor this into import plugins via polymorphism. ... but maybe this is enough. :)
                    //CSV or Excel are the only filetypes currently supported.
                    if (info.Extension == ".xls" || info.Extension == ".xlsx")
                    {
                        logger.Debug("Looks like an excel file!");
                        ExcelReader reader = new ExcelReader(newFileName);
                        //ExcelReader doesn't support starting on a certain line for column names...  we always assume col 1
                        data.columns = reader.getColumns();
                        data.rows = reader.getData().First().Table;
                        reader.close();
                    } else if (info.Extension == ".csv")
                    {
                        logger.Debug("Looks like a csv file!");
                        Int32 StartOnLine = Convert.ToInt32(provider.FormData.Get("StartOnLine")); //only applicable to T/CSV
                        CSVReader reader = new CSVReader(newFileName);
                        data = reader.getImportDataResult(StartOnLine); // we do it all in one.
                    }
                    else if (info.Extension == ".tsv")
                    {
                        logger.Debug("Looks like a tsv file!");
                        Int32 StartOnLine = Convert.ToInt32(provider.FormData.Get("StartOnLine")); //only applicable to T/CSV
                        TSVReader reader = new TSVReader(newFileName);
                        data = reader.getImportDataResult(StartOnLine); // we do it all in one.
                    }

                    else
                    {
                        logger.Debug("Looks like an unknown file!");
                        throw new Exception("File type not compatible.  We can do Excel (xls/xslx), CSV (csv) and TSV (tsv).");
                    }

                    string result = JsonConvert.SerializeObject(data);

                    //TODO: actual error/success message handling
                    //string result = "{\"message\": \"Success\"}";

                    HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

                    return resp;

                });

            return task;

        }

        //UploadProjectFile - add a file to this project.
        /**
         * Handle uploaded files
         * IEnumerable<File>
         */
        public Task<HttpResponseMessage> UploadProjectFile()
        {
            logger.Debug("starting to process incoming files.");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);

            logger.Debug("saving files to location: " + root);
            logger.Debug(" and the root url = " + rootUrl);

            var provider = new MultipartFormDataStreamProvider(root);

            User me = AuthorizationManager.getCurrentUser();

            var db = ServicesContext.Current;

            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(o =>
                {

                    if (o.IsFaulted || o.IsCanceled)
                    {
                        logger.Debug("Error: " + o.Exception.Message);
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, o.Exception));
                    }

                    //Look up our project
                    Int32 ProjectId = Convert.ToInt32(provider.FormData.Get("ProjectId"));
                    logger.Debug("And we think the projectid === " + ProjectId);

                    Project project = db.Projects.Find(ProjectId);

                    if (project == null)
                        throw new Exception("Project ID not found: " + ProjectId);

                    if (!project.isOwnerOrEditor(me))
                        throw new Exception("Authorization error.");

                    //Now iterate through the files that just came in
                    List<services.Models.File> files = new List<services.Models.File>();

                    foreach (MultipartFileData file in provider.FileData)
                    {

                        logger.Debug("Filename = " + file.LocalFileName);
                        logger.Debug("Orig = " + file.Headers.ContentDisposition.FileName);
                        logger.Debug("Name? = " + file.Headers.ContentDisposition.Name);

                        //var fileIndex = getFileIndex(file.Headers.ContentDisposition.Name); //"uploadedfile0" -> 0
                        var fileIndex = "0";
                        logger.Debug("Fileindex = " + fileIndex);
                        var filename = file.Headers.ContentDisposition.FileName;
                        filename = filename.Replace("\"", string.Empty);

                        if (!String.IsNullOrEmpty(filename))
                        {
                            try
                            {
                                var newFileName = ActionController.relocateProjectFile(
                                                file.LocalFileName,
                                                ProjectId,
                                                filename,
                                                false);

                                var info = new System.IO.FileInfo(newFileName);

                                services.Models.File newFile = new services.Models.File();
                                newFile.Title = provider.FormData.Get("Title"); //"Title_1, etc.
                                logger.Debug("Title = " + newFile.Title);

                                newFile.Description = provider.FormData.Get("Description"); //"Description_1, etc.
                                logger.Debug("Desc = " + newFile.Description);

                                newFile.Name = info.Name;//.Headers.ContentDisposition.FileName;
                                newFile.Link = rootUrl + "/servicesSTAGE/uploads/" + ProjectId + "/" + info.Name; //file.LocalFileName;
                                newFile.Size = (info.Length / 1024).ToString(); //file.Headers.ContentLength.ToString();
                                newFile.FileTypeId = FileType.getFileTypeFromFilename(info);
                                newFile.UserId = me.Id;
                                logger.Debug(" Adding file " + newFile.Name + " at " + newFile.Link);

                                files.Add(newFile);
                            }
                            catch (Exception e)
                            {
                                logger.Debug("Error: " + e.ToString());
                            }
                        }
                    }

                    List<services.Models.File> thefiles = new List<services.Models.File>();

                    //Add files to database for this project.
                    if (files.Count() > 0)
                    {
                        logger.Debug("woot -- we have file objects to save");
                        foreach (var file in files)
                        {
                            project.Files.Add(file);
                            thefiles.Add(file);
                        }
                        db.Entry(project).State = EntityState.Modified;
                        db.SaveChanges();
                        
                    }

                    logger.Debug("Done saving files.");
                    var result = JsonConvert.SerializeObject(thefiles);
                    HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");  //to stop IE from being stupid.

                    return resp;
                    
                    

                });

            return task;

        }


        [HttpPost]
        public HttpResponseMessage SaveProject(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;
            
            var in_project = json.Project.ToObject<Project>();
            
            if (in_project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            User me = AuthorizationManager.getCurrentUser();

            List<MetadataValue> metadata = new List<MetadataValue>();

            foreach (var jmv in json.Project.Metadata)
            {
                var mv = jmv.ToObject<MetadataValue>();
                mv.UserId = me.Id;
                metadata.Add(mv);
                logger.Debug("Found new metadata: " + mv.MetadataPropertyId + " + + " + mv.Values);
            }


            if (in_project.Id == 0) //is it a NEW project or editing?
            {
                in_project.OrganizationId = Organization.DEFAULT_ORGANIZATION_ID;
                in_project.OwnerId = me.Id;
                in_project.CreateDateTime = DateTime.Now;
                in_project.ProjectTypeId = ProjectType.DEFAULT_PROJECT_TYPE;

                db.Projects.Add(in_project);
                db.SaveChanges();
                in_project.Metadata = metadata;
                db.SaveChanges(); //not sure if this is required.
                logger.Debug("Created new project: " + in_project.Id);
            }
            else
            {
                //find the existing project
                Project project = db.Projects.Find(in_project.Id);
                if (project == null)
                    throw new Exception("Configuration error.");

                //ok if they are editing the project, they can only edit projects they own or are editors
                if (!project.isOwnerOrEditor(me))
                    throw new Exception("Authorization error.");

                //map our properties.
                project.Description = in_project.Description;
                project.EndDate = in_project.EndDate;
                project.StartDate = in_project.StartDate;
                project.Name = in_project.Name;

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug("Saved property changes to project: " + project.Id);

                project.Metadata = metadata;
                db.SaveChanges();

            }

            HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            return resp;

        }


        /*
         * Executes a query from the given criteria in our json object on the given dataset 
         * datafieldsource can be a dataset or a datastore
         * returns: DataTable of results
         */

        private DataTable getQueryResults(dynamic datafieldsource, dynamic json)
        {
            var fields = datafieldsource.Fields;
            
            var conditions = new List<string>();

            //logger.Debug(json.Fields);
            //logger.Debug(json.Fields.ToString());

            //fields in the criteria
            foreach (var item in json.Fields)
            {
                logger.Debug(item);
                logger.Debug("Colname!: " + item.DbColumnName);

                //spin through each of our dataset/datastore fields and find a match, adding it to our criteria...
                foreach (var field in fields)
                {
                    //logger.Debug(" -- alright now I think we're looking up field with id: " + item.Id);

                    if (field.Id != item.Id.ToObject<int>())
                        continue;

                    //logger.Debug("Looked up field: " + field.DbColumnName);

                    //if (field == null)
                    //    throw new Exception("Field not configured properly: " + item.Value);

                    string ControlType = field.ControlType.ToString(); //hmm, can't use directly in a switch.

                    //now add field criteria to our list...
                    switch (ControlType)
                    {
                        case "number":
                        case "currency":
                            logger.Debug("A number or currency");
                            conditions.Add(field.DbColumnName + item.Value); //>100
                            break;

                        case "text":
                        case "textarea":
                            logger.Debug("A txt");
                            var conditional = " = ";
                            if (item.Value.ToString().Contains("%"))
                                conditional = " LIKE ";

                            conditions.Add(field.DbColumnName + conditional + "'" + item.Value + "'");
                            break;

                        case "multiselect":
                            logger.Debug("A MULTIselect:" + item.Value);
                            if (item.Value == null)
                                break;

                            dynamic mselect_val = item.Value; //array

                            //iterate and construct strings.
                            List<string> ms_condition = new List<string>();
                            foreach (var ms_item in mselect_val)
                            {
                                ms_condition.Add(field.DbColumnName + " LIKE '%\"" + ms_item + "\"%'");
                            }

                            conditions.Add("(" + string.Join(" OR ", ms_condition) + ")");

                            break;
                        case "select":
                            logger.Debug("A select:" + item.Value);
                            if (item.Value == null)
                                break;

                            dynamic select_val = item.Value; //array

                            conditions.Add(field.DbColumnName + " in('" + string.Join("','", select_val) + "')");
                            break;
                        case "date":
                        case "datetime":
                            logger.Debug("A date!: ");
                            if (item.Value.ParamFieldDateType == "between") //otherwise, do nothing with this criteria
                            {
                                conditions.Add(field.DbColumnName + " between '" + item.Value.BetweenFromFieldDate + "' and '" + item.Value.BetweenToFieldDate + "'");
                            }


                            break;
                    }
                }

            }

            //DATE criteria
            if (json.DateSearchType == "between")
            {
                conditions.Add("ActivityDate BETWEEN CONVERT(Date, '" + json.FromDate + "') AND DATEADD(DAY,1,CONVERT(Date, '" + json.ToDate + "'))");
            }

            //LOCATION criteria
            if (json.Locations != "[\"all\"]")
            {
                logger.Debug(json.Locations);
                var locations = new List<string>();
                var locations_in = JArray.Parse(json.Locations.ToObject<string>());
                foreach (var item in locations_in)
                {
                    locations.Add(item.ToObject<string>());
                }
                conditions.Add("LocationId IN (" + string.Join(",", locations.ToArray()) + ")");
            }

            //QASTATUS
            if (json.QAStatusId != "all")
            {
                conditions.Add("ActivityQAStatusId=" + json.QAStatusId);
            }

            //ROWQASTATUS
            if (json.RowQAStatusId != null && json.RowQAStatusId != "[\"all\"]")
            {
                logger.Debug(json.RowQAStatusId);
                var rowqas = new List<string>();
                var rowqas_in = JArray.Parse(json.RowQAStatusId.ToObject<string>());
                foreach (var item in rowqas_in)
                {
                    rowqas.Add(item.ToObject<string>());
                }
                conditions.Add("QAStatusId IN (" + string.Join(",", rowqas.ToArray()) + ")");
            }


            /*

            var all_details = from d in db.AdultWeir_Detail
                              join a in db.Activities on d.ActivityId equals a.Id
                              where d.RowStatusId == DataDetail.ROWSTATUS_ACTIVE
                              join h2 in
                                (
                                    from hh in db.AdultWeir_Detail
                                    where hh.EffDt <= DateTime.Now
                                    group hh by new { hh.ActivityId, hh.RowId } into cig
                                    select new { ActivityId = cig.Key.ActivityId, RowId = cig.Key.RowId, EffDt = cig.Max(ed => ed.EffDt) }
                                ) on new { d.ActivityId, d.RowId, d.EffDt } equals new { h2.ActivityId, h2.RowId, h2.EffDt }
                            select d;

            var criteria_string = string.Join(" AND ", conditions.ToArray());
            logger.Debug(criteria_string);

            all_details = all_details.Where(criteria_string);

            return all_details;
             * */

            var datatable_prefix = "UNKNOWN";

            if(datafieldsource is Dataset)
                datatable_prefix = datafieldsource.Datastore.TablePrefix;
            else
	            datatable_prefix = datafieldsource.TablePrefix;

            string query = "SET QUOTED_IDENTIFIER OFF; SELECT " + datafieldsource.getExportSelectString() + " from " + datatable_prefix + "_VW WHERE 1=1";

            if(datafieldsource is Dataset)
                query += " AND DatasetId = " + datafieldsource.Id ;

            var criteria_string = string.Join(" AND ", conditions.ToArray());

            if (criteria_string != "")
                query += " AND " + criteria_string;

            logger.Debug("final query = " + query);
            //query = "SET QUOTED_IDENTIFIER OFF; SELECT * from AdultWeir_VW WHERE DatasetId = 5 AND Species=\"CHS\"";


            //open a raw database connection...
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;


        }
    }

}
