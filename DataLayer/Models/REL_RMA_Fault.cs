using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class REL_RMA_Fault : ItemDBHandler
    {
        public REL_RMA_Fault(int id)
        : base(id)
        {

        }

        public REL_RMA_Fault() { }

        public int? RMAId { get; set; }

        public int? FaultId { get; set; }

        public string Observations { get; set; }
        public string StepsUndertaken { get; set; }
        public bool? IsOpen { get; set; }
        public bool? IsRemoved { get; set; }



    }
}