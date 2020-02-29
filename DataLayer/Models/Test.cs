using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Test : ItemDBHandler
    {
        public Test(int id)
        : base(id)
        {

        }
        public Test() { }

        public string Description { get; set; }

        public int? PartFamilyId { get; set; }

    }
}