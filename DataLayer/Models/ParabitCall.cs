using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ParabitCall : ItemDBHandler
    {
        public ParabitCall(int id)
        : base(id)
        {

        }
        public ParabitCall() { }

        public bool WasParabitCalled { get; set; }
        public string DispatchNumber { get; set; }
        public string CallDate { get; set; }
    }
}