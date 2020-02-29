using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PasswordType : ItemDBHandler
    {
        public PasswordType(int id)
        : base(id)
        {

        }
        public PasswordType() { }

        public string Description { get; set; }

        public int? Duration { get; set; }

    }
}