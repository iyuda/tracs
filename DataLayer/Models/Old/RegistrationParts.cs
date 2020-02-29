using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RegistrationParts : ItemDBHandler
    {
        public RegistrationParts(int? id=0)
           : base(id)
        {

        }
        public RegistrationParts()
        {
        }
        // Property names that start with underscore are are the ones that do not have corresponding database fields
        public string _PartNo { get; set; }
        public string _PartDescription { get; set; }
 

        public string serial_no { get; set; }
        public int seq_no { get; set; }
        public string part_types_id { get; set; }
        public int? registration_id { get; set; }   
        public string image_location { get; set; }
        public string image_description { get; set; }
        
        public override void SaveChildren()
        {
        }
       
    }
}
