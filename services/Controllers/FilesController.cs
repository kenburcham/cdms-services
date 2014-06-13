using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using services.Models;
using services.Resources;

namespace services.Controllers
{
    public class FilesController : ApiController
    {

        private readonly IFileRepository _fileRepository;

        public FilesController()
        {
            _fileRepository = new EFFileRepository(); //TODO: use DI
        }

        // GET api/file
        public IEnumerable<File> Get()
        {
            throw new NotImplementedException();
            //return _fileRepository.GetFiles();
        }

        // GET api/file/5
        public File Get(int id)
        {
            return _fileRepository.GetFile(id);
        }

        // POST api/file
        public HttpResponseMessage Post(File file)
        {
            return _fileRepository.PostFile(file);
        }

        // PUT api/file/5
        public HttpResponseMessage Put(File file)
        {
            return _fileRepository.PutFile(file);
        }

        // DELETE api/file/5
        public HttpResponseMessage Delete(int id)
        {
            return _fileRepository.DeleteFile(id);
        }
    }
}
