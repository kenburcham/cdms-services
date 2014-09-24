using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using services.Models;
using Newtonsoft.Json;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace services.Resources
{
    public class LookupFieldHelper
    {
        //executes the query and returns the "Label" as an array
        //  this is intended for use in dynamically returning lookup or multilookup controltype fields.
        public static string getPossibleValues(string raw_query)
        {
            if (raw_query == null)
                return null;

            string pv_value = raw_query.ToUpper();
            string query = pv_value.Replace("DROP", "").Replace("DELETE", "");

            List<string> values = new List<string>();

            //open a raw database connection...
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServicesContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    var rdr = cmd.ExecuteReader();

                    while(rdr.Read())
                    {
                        values.Add(rdr["Label"].ToString());
                    }
                }
            }

            return JsonConvert.SerializeObject(values); // "[\"ABC\",\"123\"]";
        }
    }
}