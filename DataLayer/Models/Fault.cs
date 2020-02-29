using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Fault : ItemDBHandler
    {
        public Fault(int id)
        : base(id)
        {

        }
        public Fault() { }

        [JsonProperty(PropertyName = "FaultDescription")]
        public string Description { get; set; }

        public static List<Fault> fullList{ get; set; }
    }
}