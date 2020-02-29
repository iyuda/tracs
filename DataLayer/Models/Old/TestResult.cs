using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class TestResult : ItemDBHandler
    {
        
        public TestResult() { }

        public int? REL_ReturnedPart_ComplaintId { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        public DateTime? OriginalShippedDate { get; set; }

        public int? RMATechId { get; set; }

        public bool? IsPhisicalDamage { get; set; }

        public int? ReturnType { get; set; }

        public bool? IsConfirmed { get; set; }

    }
}