using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using services.Models;
using services.Models.Data;
using services.Resources;

namespace services.Controllers
{
    /*
     * This controller provides a place for RPC-type calls.
     * Ken Burcham 8/9/2013
     */
    [Authorize]
    public class ActionController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [AllowAnonymous]
        [HttpPost]
        public string SystemLog(JObject jsonData)
        {
            dynamic json = jsonData;
            
            if(jsonData.GetValue("Type").ToString() == "AUDIT")
            {
                logger.Info(jsonData.GetValue("Message")); 
            }
            else{
                logger.Error(jsonData.GetValue("Message"));
            }

            return "{Message: 'Success'}";
        }


        //returns empty list if none found...
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Dataset> ProjectDatasets(int Id)
        {
            var result = new List<Dataset>();

            var ndb = ServicesContext.Current;

             var datasets = ndb.Datasets.Where(o => o.ProjectId == Id);

            return datasets;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Activity> DatasetActivities(int Id)
        {
            var result = new List<Activity>();

            var ndb = ServicesContext.Current;

            var activities = ndb.Activities.Where(o => o.DatasetId == Id);

            return activities;
        }

        [AllowAnonymous]
        [HttpGet]
        public dynamic DatasetData(int Id)
        {
            var db = ServicesContext.Current;
            Activity activity = db.Activities.Find(Id);
            if (activity == null)
                throw new Exception("Configuration Error");

            System.Type type = db.GetTypeFor(activity.Dataset.Datastore.TablePrefix);

            //instantiate by name
            return Activator.CreateInstance(type, activity.Id);
        }


        //returns empty list if none found...
        [HttpGet]
        public List<File> ProjectFiles(int ProjectId)
        {
            var result = new List<File>();

            var ndb = ServicesContext.Current;

            Project project = ndb.Projects.Find(ProjectId);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            result = project.Files; 

            
            return result;
        }

        //we will overwrite any of the keys that exist in the request
        [HttpPost]
        public HttpResponseMessage SaveUserPreference(JObject jsonData)
        {
            //string result = "{message: 'Success'}"; //TODO!
            //var resp = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);
            //resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");

            var ndb = ServicesContext.Current;

            dynamic json = jsonData;
            JObject jpref = json.UserPreference;
            var pref = jpref.ToObject<UserPreference>();

            logger.Debug("Hey we have a user preference save!" + pref.Name + " = " + pref.Value);

            User me = AuthorizationManager.getCurrentUser();

            logger.Debug("Userid = " + me.Id);

            pref.UserId = me.Id; // you can only save preferences that are your own.

            //fetch user with preferences from the database -- really want a round-trip here.
            me = ndb.User.Find(me.Id);
                
            logger.Debug("Number of existing prefs for user = " + me.UserPreferences.Count());

            UserPreference match = me.UserPreferences.Where(x => x.Name == pref.Name).SingleOrDefault();

            if (match != null)
            {
                match.Value = pref.Value;
                ndb.Entry(match).State = EntityState.Modified; 
            }
            else
            { 
                me.UserPreferences.Add(pref);
            }

            try
            {
                ndb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Debug("Something went wrong saving the preference: " + e.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        public Project SaveProjectDetails(JObject jsonData)
        {
            Project project = null;

            var db = ServicesContext.Current;

            dynamic json = jsonData;
            JObject jproject = json.Project;
            JObject jlocation = json.Location;

            var in_project = jproject.ToObject<Project>();
            var in_location = jlocation.ToObject<Location>();

            if (in_project.Id == 0 || in_location.SdeFeatureClassId == 0 || in_location.SdeObjectId == 0)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            logger.Debug("incoming location objectid == " + in_location.SdeObjectId);

            project = db.Projects.Find(in_project.Id);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var locations = from loc in db.Location
                            where loc.SdeFeatureClassId == in_location.SdeFeatureClassId
                                && loc.SdeObjectId == in_location.SdeObjectId
                            select loc;

            Location location = locations.FirstOrDefault();

            if (location == null)
            {
                //then try to add it to the system so we can add it to our project
                logger.Debug("incoming Location doesn't exist, we will create it and link to it.");
                location = new Location();
                location.SdeFeatureClassId = in_location.SdeFeatureClassId;
                location.SdeObjectId = in_location.SdeObjectId;
                location.LocationTypeId = LocationType.PROJECT_TYPE;
                db.Location.Add(location);
                db.SaveChanges(); //we save the changes so that we have the id.
                logger.Debug("Saved a new location with id: " + location.Id);
            }

            logger.Debug(" and the locationid we are linking to will be " + location.Id);

            //link our project to that location if it isn't already
            if (project.Locations.Where(o => o.Id == location.Id).SingleOrDefault() == null)
            {
                logger.Debug("Project didn't have that location ... adding it.");
                project.Locations.Add(location);
            }
            else
            {
                logger.Debug("Project already has that location... why do we even bother?! (" + location.Id + ")");
            }

            User me = AuthorizationManager.getCurrentUser();

            //set project owner
            //project.OwnerId = me.Id; //this shouldn't be done here, but rather when we initially create the project.

            //db.Entry(project).State = EntityState.Modified; //shouldn't be necessary...
                
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
            project.Metadata = metadata;

            db.SaveChanges();

            //need to refetch project -- otherwise it is old data
            //db.Entry(project).Reload();

            //logger.Debug("ok we saved now we are reloading...");

            db = ServicesContext.RestartCurrent;
            project = db.Projects.Where(o => o.Id == in_project.Id).SingleOrDefault();
            db = ServicesContext.RestartCurrent;
            project = db.Projects.Where(o => o.Id == in_project.Id).SingleOrDefault();
            db = ServicesContext.RestartCurrent;
            project = db.Projects.Where(o => o.Id == in_project.Id).SingleOrDefault();

            
            foreach (var mv in project.Metadata)
            {
                logger.Debug(" out --> " + mv.MetadataPropertyId + " === " + mv.Values);
            }
            

            //logger.Debug(JsonConvert.SerializeObject(project));

            return project; // JsonConvert.SerializeObject(project); //return our newly setup project.

        }

        /**
         * Handle uploaded files
         * IEnumerable<File>
         */
        public Task<HttpResponseMessage> PostFiles()
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

                    //TODO: collaborators?
                    //security check :: you can only edit your own projects
                    if (project.Owner.Id != me.Id)
                    {
                        throw new Exception("NotAuthorized: You can only edit projects you own.");
                    }


                    //Now iterate through the files that just came in
                    List<File> files = new List<File>();

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
                                var newFileName = relocateProjectFile(
                                                file.LocalFileName,
                                                ProjectId,
                                                filename);

                                var info = new System.IO.FileInfo(newFileName);

                                File newFile = new File();
                                newFile.Title = provider.FormData.Get("Title_" + fileIndex); //"Title_1, etc.
                                logger.Debug("Title = " + newFile.Title);

                                newFile.Description = provider.FormData.Get("Description_" + fileIndex); //"Description_1, etc.
                                logger.Debug("Desc = " + newFile.Description);

                                newFile.Name = info.Name;//.Headers.ContentDisposition.FileName;
                                newFile.Link = rootUrl + "/services/uploads/" + ProjectId + "/" + info.Name; //file.LocalFileName;
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

                    //Add files to database for this project.
                    if (files.Count() > 0)
                    {
                        logger.Debug("woot -- we have file objects to save");
                        foreach (var file in files)
                        {
                            project.Files.Add(file);
                        }
                        db.Entry(project).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    logger.Debug("Done saving files.");

                    //TODO: actual error/success message handling
                    string result = "{message: 'Success'}";

                    HttpResponseMessage resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");

                    return resp;

                });

            return task;

        }

        //"uploadedfile0" -> 0
        public static object getFileIndex(string name)
        {
            var fileIndex = name.Replace("\"", string.Empty);
            fileIndex = fileIndex.Replace("uploadedfile", string.Empty);
            return fileIndex;
        }

        /**
         * takes current filename, project id and original filename and moves the file
         * from something like: D:\WebSites\GISInternet\services\uploads\BodyPart_c0a2f6f8-446b-42ee-88ab-2f2f3ace1e75
         * to D:\WebSites\GISInternet\services\uploads\3729\originalFilename.pdf 
         * where 3729 is the projectid folder that will be created if it doesn't exist.
         */
        public static string relocateProjectFile(string current_fullfile, int ProjectId, string orig_fullfile, bool makeUnique = false)
        {
            string new_filename = current_fullfile;

            orig_fullfile = orig_fullfile.Replace("\"", string.Empty);

            if (String.IsNullOrEmpty(orig_fullfile))
                throw new Exception("Original filename path is not given.");
            
            logger.Debug("Incoming current: " + current_fullfile);
            logger.Debug("Original file: " + orig_fullfile);
                    
            string directory = System.IO.Path.GetDirectoryName(current_fullfile);
            string orig_filename = System.IO.Path.GetFileName(orig_fullfile);

            //unless we want to make a UNIQUE filename (like for importing)
            if (makeUnique)
            {
                orig_filename = System.IO.Path.GetFileNameWithoutExtension(orig_fullfile) + "_" + System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + System.IO.Path.GetExtension(orig_fullfile);
            }

            string project_directory = directory + @"\" + ProjectId;

            logger.Debug("New target file: " + orig_filename);

            //first, ensure we have a projectid folder (will auto-create if necessary)
            logger.Debug("Creating (if necessary) project directory: " + project_directory);
            System.IO.Directory.CreateDirectory(project_directory);

            //now move the file from where it is to the project directory with the new name.
            new_filename = project_directory + @"\" + orig_filename;
            logger.Debug("Moving uploaded file to: " + new_filename);
            System.IO.File.Move(current_fullfile, new_filename);
            
            return new_filename;

        }




        [HttpGet]
        [AllowAnonymous]
        [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public User WhoAmI()
        {
            logger.Debug("whoami?");

            logger.Debug("might be --> " + System.Web.HttpContext.Current.Request.LogonUserIdentity.Name);
            if (User.Identity.IsAuthenticated)
                logger.Debug("  it says we are authenticated.");

            logger.Debug("Can we get our user?");

            User me = AuthorizationManager.getCurrentUser();

            if (me == null)
            {
                logger.Debug("nope");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
            }

            logger.Debug("yep! you are "+me.Username);
            
            var ndb = ServicesContext.Current;
            me = ndb.User.Find(me.Id);

            return me;
        }

        



    }
}
