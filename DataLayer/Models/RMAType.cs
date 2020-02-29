using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RMAType : ItemDBHandler
    {
        public RMAType(int id)
        : base(id)
        {

        }
        public RMAType() { }

        public string Description { get; set; }
        public static List<RMAType> fullList { get; set; }
    }
}