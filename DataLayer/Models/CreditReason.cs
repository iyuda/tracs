using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CreditReason : ItemDBHandler
    {
        public CreditReason(int id)
        : base(id)
        {

        }
        public CreditReason() { }


        public string Description { get; set; }
        public static List<CreditReason> fullList { get; set; }
    }
}