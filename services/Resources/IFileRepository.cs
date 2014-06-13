using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using services.Models;

namespace services.Resources
{
    public interface IFileRepository
    {
        IEnumerable<File> GetFiles();

        File GetFile(int id);

        HttpResponseMessage PostFile(File File);

        HttpResponseMessage PutFile(File File);

        HttpResponseMessage DeleteFile(int id);
    }
}