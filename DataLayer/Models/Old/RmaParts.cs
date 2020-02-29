using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RmaParts :ItemDBHandler
    {
        public RmaParts(int? id=0)
           : base(id)
        {

        }
        public RmaParts()
        {
        }
        // Property names that start with underscore are are the ones that do not have corresponding database fields
        public string _PartNo { get; set; }
        public string _PartDescription { get; set; }
        public string _status { get; set; }

        public string serial_no { get; set; }
        public int seq_no { get; set; }
        public string part_types_id { get; set; }
        public int? rma_id { get; set; }
        public string image_path { get; set; }
        public string image_description { get; set; }
        public string status_id { get; set; }
        public string additional_notes { get; set; }
        public override void SaveChildren()
        {
        }
       
    }
}
