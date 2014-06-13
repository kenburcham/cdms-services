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
using services.Models;

namespace services.Controllers
{
    public class ProjectTypesController : ApiController
    {

        // GET api/ProjectTypes
        public IEnumerable<ProjectType> GetProjectTypes()
        {
            var db = ServicesContext.Current;
            return db.ProjectType.AsEnumerable();
        }

        // GET api/ProjectTypes/5
        public ProjectType GetProjectType(int id)
        {
            var db = ServicesContext.Current;
            ProjectType projecttype = db.ProjectType.Find(id);
            if (projecttype == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return projecttype;
        }

        // PUT api/ProjectTypes/5
        public HttpResponseMessage PutProjectType(int id, ProjectType projecttype)
        {
            var db = ServicesContext.Current;
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != projecttype.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(projecttype).State = EntityState.Modified;

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

        // POST api/ProjectTypes
        public HttpResponseMessage PostProjectType(ProjectType projecttype)
        {
            var db = ServicesContext.Current;
            if (ModelState.IsValid)
            {
                db.ProjectType.Add(projecttype);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, projecttype);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = projecttype.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/ProjectTypes/5
        public HttpResponseMessage DeleteProjectType(int id)
        {
            var db = ServicesContext.Current;
            ProjectType projecttype = db.ProjectType.Find(id);
            if (projecttype == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ProjectType.Remove(projecttype);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, projecttype);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}