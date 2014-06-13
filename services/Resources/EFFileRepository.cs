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

namespace services.Resources
{
    public class EFFileRepository : IFileRepository
    {
        // GET api/File
        public IEnumerable<File> GetFiles()
        {
            var db = ServicesContext.Current;
            var files = db.Files.Include(f => f.Project).Include(f => f.User);
            return files.AsEnumerable();
        }

        // GET api/File/5
        public File GetFile(int id)
        {
            var db = ServicesContext.Current;
            File file = db.Files.Find(id);
            if (file == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("File not found")
                });
            }

            return file;
        }

        // PUT api/File/5
        public HttpResponseMessage PutFile(File file)
        {
            var db = ServicesContext.Current;
            if (file == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("File not valid")
                });
            }

            db.Entry(file).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Database error.")
                });
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = file.Id }));
            return response;
        }

        // POST api/File
        public HttpResponseMessage PostFile(File file)
        {
            var db = ServicesContext.Current;
            if (file != null)
            {
                db.Files.Add(file);
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("File id does not match")
                });
            }
        }

        // DELETE api/File/5
        public HttpResponseMessage DeleteFile(int id)
        {
            var db = ServicesContext.Current;
            File file = db.Files.Find(id);
            if (file == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("File not found")
                });
            }

            db.Files.Remove(file);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Database save error")
                });
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}