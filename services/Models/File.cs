using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models
{
    public class File
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } //myfile.pdf
        public string Title { get; set; } //My File
        public string Description { get; set; } //This is my file.
        public DateTime UploadDate { get; set; }
        public string Size { get; set; }
        public string Link { get; set; } // /repo/files/myfile.pdf
        public int FileTypeId { get; set; }

        [JsonIgnore]
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
        public virtual FileType FileType { get; set; }

        public File()
        {
            //milliseconds causes us trouble when converting to javascript date later.  so we just want seconds.
            var now = DateTime.UtcNow;
            UploadDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);
        }

    }
}