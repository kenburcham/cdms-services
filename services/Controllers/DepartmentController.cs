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
    public class DepartmentController : ApiController
    {
        // GET api/Department
        public IEnumerable<Department> GetDepartments()
        {
            var db = ServicesContext.Current;
            var departments = db.Departments.Include(d => d.Organization);
            return departments.AsEnumerable();
        }

        // GET api/Department/5
        public Department GetDepartment(int id)
        {
            var db = ServicesContext.Current;
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return department;
        }

        // PUT api/Department/5
        public HttpResponseMessage PutDepartment(int id, Department department)
        {
            var db = ServicesContext.Current;
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != department.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(department).State = EntityState.Modified;

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

        // POST api/Department
        public HttpResponseMessage PostDepartment(Department department)
        {
            var db = ServicesContext.Current;
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, department);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = department.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Department/5
        public HttpResponseMessage DeleteDepartment(int id)
        {
            var db = ServicesContext.Current;
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Departments.Remove(department);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, department);
        }

        protected override void Dispose(bool disposing)
        {
            var db = ServicesContext.Current;
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}