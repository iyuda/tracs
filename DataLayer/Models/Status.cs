using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Status : ItemDBHandler
    {
        public Status(int id)
        : base(id)
        {

        }
        public Status() { }

        public string Description { get; set; }

        public string PublicDescription { get; set; }

        public bool? IsPublic { get; set; }

    }
}