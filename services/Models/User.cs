using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace services.Models
{
    public class User
    {
        public const int DEFAULT_ORGANIZATIONID = 1;
        public const int DEFAULT_DEPARTMENTID = 1;

        public int Id { get; set; }
        public int? OrganizationId { get; set; }
        public int? DepartmentId { get; set; }
        public string GUID { get; set; }
        public string Fullname { get; set; }
        public string Roles { get; set; }
        public string ProfileImageUrl { get; set; }
        
        public string Username { get; set; }
        public string Description { get; set; }
        public DateTime LastLogin { get; set; }

        public virtual Department Department { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual List<UserPreference> UserPreferences { get; set; }

        [JsonIgnore]
        [InverseProperty("Editors")]
        public virtual List<Project> ProjectEditor { get; set; }

        public void BumpLastLoginDate(){
            var now = DateTime.UtcNow;
            LastLogin = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);
        }

        public User(string i_username) {
            Username = i_username;
            Fullname = i_username;
            OrganizationId = DEFAULT_ORGANIZATIONID;
            DepartmentId = DEFAULT_DEPARTMENTID;
        }

        public User() { }
    }
}