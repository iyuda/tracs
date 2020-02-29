using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Registrations : ItemDBHandler
    {
        public Registrations(int id)
           : base(id)
        {

        }
        public Registrations() { }

        public string registration_no { get; set; }
        public string date_installed { get; set; }
        public int? bankinfo_id { get; set; }
        public int? tech_info_id { get; set; }

   
        
        public override void SaveChildren()
        {

        }
    }
}
