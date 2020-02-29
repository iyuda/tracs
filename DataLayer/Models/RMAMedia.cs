using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace DataLayer
{
    [DataContract]
    public class RMAMedia : ItemDBHandler
    {
        public RMAMedia(int id)
        : base(id)
        {

        }
        public RMAMedia() { }

        public int? RMAId { get; set; }
        [DataMember]
        public string Filename { get; set; }
        [DataMember]
        public object Content { get; set; }
        [DataMember]
        public bool? isVideo { get; set; }
        [ScriptIgnore]
        public bool? IsRemoved { get; set; }
    }
}