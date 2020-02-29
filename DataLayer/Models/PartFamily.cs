using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PartFamily : ItemDBHandler
    {
        public PartFamily(int id)
        : base(id)
        {

        }
        public PartFamily() { }

        public string Description { get; set; }

    }
}