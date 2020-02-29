using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PartType : ItemDBHandler
    {
        public PartType(int id)
        : base(id)
        {

        }
        public PartType() { }

        public string PartNumber { get; set; }
        public string PartDescription { get; set; }
        
   
        public static List<PartType> fullList { get; set; }
        
        
    }
}