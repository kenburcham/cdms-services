using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using services.Models;
using services.Resources;
using NLog;
using System.Web.Mvc;
using services.Resources.Filters;

namespace services.Controllers
{
    [System.Web.Http.Authorize]
    public class ProjectsController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger(); 

        // GET api/Projects
        //[ProjectAuth]
        [System.Web.Http.AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public IEnumerable<Project> GetProjects()
        {
            var db = ServicesContext.Current;
            //logger.Info("GetProjects called!");

            //this is one way to neck down what gets returned... loading all the files is very time consuming for the big list...
            /*
            var results = db.Projects.Select(p => new
            {
                p.Id,
                p.ProjectType,
                p.Name,
                p.Description,
                p.CreateDateTime,
                p.OrganizationId,
                p.OwnerId,
                p.Metadata
            });
            */
            //return results.AsEnumerable();

            return db.Projects.OrderBy(o => o.Name).AsEnumerable();
        }

        // GET api/Projects/5
        [System.Web.Http.AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public Project GetProject(int id)
        {
            var db = ServicesContext.Current;
            logger.Info("GetProject called with id: " + id);
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return project;
        }

        // PUT api/Projects/5
        [ProjectAuth(AuthorizationManager.EDIT_PERMISSION)]
        public HttpResponseMessage PutProject(int id, Project project)
        {
            var db = ServicesContext.Current;
            logger.Info("PutProject called with id: " + id);

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != project.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var dbx = new ServicesContext(); //stupid that we have to do this.  maybe i'm stupid and there is a better way?! //TODO (we get ObjectStateManager error if we lookup and then change a project by id)
            var ownerid = dbx.Projects.Where(o => o.Id == id).FirstOrDefault().OwnerId;
            dbx.Dispose();

            //handle copying the ownerid from the existing project.
            project.OwnerId = ownerid;
            logger.Debug("PUT with owner id = " + ownerid + " for project " + project.Id);

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Projects
        [ProjectAuth(AuthorizationManager.EDIT_PERMISSION)]
        public HttpResponseMessage PostProject(Project project)
        {
            var db = ServicesContext.Current;
            logger.Info("PostProject called with project id: " + project.Id);
            project.CreateDateTime = DateTime.Now;

            User me = AuthorizationManager.getCurrentUser();
            project.OwnerId = me.Id;

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, project);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = project.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Projects/5
        [ProjectAuth(AuthorizationManager.EDIT_PERMISSION)]
        public HttpResponseMessage DeleteProject(int id)
        {
            var db = ServicesContext.Current;
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            project.deleteRelatedData();
            db.Projects.Remove(project);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }


            HttpResponseMessage response =  Request.CreateResponse(HttpStatusCode.OK, "Success");

            return response;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}