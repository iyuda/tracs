using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DataLayer
{
    [DataContract]
    public class ReturnedPart : ItemDBHandler
    {
        public ReturnedPart(int id)
        : base(id)
        {
            LoadRelations();
        }
        public ReturnedPart() {
         
        }
        [DataMember]
        public int? PartId { get; set; }
        [DataMember]
        public string PartNumber { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }

        [ScriptIgnore]
        public int? RMAId { get; set; }
        
        [ScriptIgnore]
        public string PartDescription { get; set; }
        [ScriptIgnore]
        public string RMANumber { get; set; }
        [ScriptIgnore]
        public string ReplacementSONumber { get; set; }
        [ScriptIgnore]
        public string FirmwareVersion { get; set; }
        [ScriptIgnore]
        public Part part { get; set; }
        [ScriptIgnore]
        public bool? IsRemoved { get; set; }

        public void LoadRelations()
        {
            LoadDetails();
        }
        public void LoadDetails()
        {
            // test area
            part = Test_GetDetails();
            return;

            parameters.Clear();
            parameters.Add("id", new object[] { this.id.ToString(), SqlDbType.Int });
            List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            part = DataHelper.GetQueryListBySP<Part>("PartGetById", sqlParameters)?[0];
        }
        private Part Test_GetDetails()
        {
            return Part.fullList?.FirstOrDefault(x => x.id == this.PartId);
            
        }
    }
}