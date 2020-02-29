using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class TempPassword  
    {
        
        public TempPassword() { }

        public int? UserId { get; set; }
        public string Password { get; set; }

    }
}