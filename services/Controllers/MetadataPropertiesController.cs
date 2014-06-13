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
    public class MetadataPropertiesController : ApiController
    {
        // GET api/Metadata
        public IEnumerable<MetadataProperty> GetMetadataProperties()
        {
            var db = ServicesContext.Current;
            var metadataproperty = db.MetadataProperty.Include(m => m.MetadataEntity);
            return metadataproperty.AsEnumerable();
        }

        // GET api/Metadata/5
        public MetadataProperty GetMetadataProperty(int id)
        {
            var db = ServicesContext.Current;
            MetadataProperty metadataproperty = db.MetadataProperty.Find(id);
            if (metadataproperty == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return metadataproperty;
        }

        // PUT api/Metadata/5
        public HttpResponseMessage PutMetadataProperty(int id, MetadataProperty metadataproperty)
        {
            var db = ServicesContext.Current;
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != metadataproperty.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(metadataproperty).State = EntityState.Modified;

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

        // POST api/Metadata
        public HttpResponseMessage PostMetadataProperty(MetadataProperty metadataproperty)
        {
            var db = ServicesContext.Current;
            if (ModelState.IsValid)
            {
                db.MetadataProperty.Add(metadataproperty);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, metadataproperty);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = metadataproperty.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Metadata/5
        public HttpResponseMessage DeleteMetadataProperty(int id)
        {
            var db = ServicesContext.Current;
            MetadataProperty metadataproperty = db.MetadataProperty.Find(id);
            if (metadataproperty == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.MetadataProperty.Remove(metadataproperty);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, metadataproperty);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}