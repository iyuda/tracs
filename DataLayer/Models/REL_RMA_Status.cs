using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class REL_RMA_Status : ItemDBHandler
    {
        public REL_RMA_Status(int id)
        : base(id)
        {

        }
        public REL_RMA_Status() { }

        public DateTime? CreatedAt { get; set; }
        public string Observations { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }
        public int? RMAId { get; set; }

        public Status status { get; set; }
    }
}