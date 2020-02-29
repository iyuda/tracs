using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [DataContract]
    public class DispatchReason : ItemDBHandler
    {
        public DispatchReason(int id)
        : base(id)
        {

        }
        
        public DispatchReason() { }

        [DataMember (Name ="DispatchReasonId")]
        [JsonProperty(PropertyName = "DispatchReasonId")]
        public override int? id { get; set; }

        
        public string Name { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "DispatchReasonDescription")]
        public string Description { get; set; }

        public static List<DispatchReason> fullList { get; set; }
    }
}