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
using NLog;
using services.Models;

namespace services.Controllers
{
    [System.Web.Http.Authorize]
    public class DatasetsController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger(); 


        // GET api/Datasets
        public IEnumerable<Dataset> GetDatasets()
        {
            var db = ServicesContext.Current;

            var datasets = db.Datasets.Include(d => d.DefaultRowQAStatus);
            return datasets.AsEnumerable();
        }

        // GET api/Datasets/5
        public Dataset GetDataset(int id)
        {
            var db = ServicesContext.Current;

            Dataset dataset = db.Datasets.Find(id);
            if (dataset == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return dataset;
        }

        // PUT api/Datasets/5
        public HttpResponseMessage PutDataset(int id, Dataset dataset)
        {
            var db = ServicesContext.Current;

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != dataset.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(dataset).State = EntityState.Modified;

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

        // POST api/Datasets
        public HttpResponseMessage PostDataset(Dataset dataset)
        {
            var db = ServicesContext.Current;

            if (ModelState.IsValid)
            {
                db.Datasets.Add(dataset);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, dataset);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = dataset.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Datasets/5
        public HttpResponseMessage DeleteDataset(int id)
        {
            var db = ServicesContext.Current;

            Dataset dataset = db.Datasets.Find(id);
            if (dataset == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Datasets.Remove(dataset);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dataset);
        }

        protected override void Dispose(bool disposing)
        {
            var db = ServicesContext.Current;

            db.Dispose();
            base.Dispose(disposing);
        }
    }
}