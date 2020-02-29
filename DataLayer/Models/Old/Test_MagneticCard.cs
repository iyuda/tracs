using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Test_MagneticCard : ItemDBHandler
    {
        
        public Test_MagneticCard() { }

        public int? REL_ReturnedPart_ComplaintId { get; set; }

        public int? MagCardRead { get; set; }

        public int? OutOf50Swipes { get; set; }

        public bool? IsPassed { get; set; }

        public string Observations { get; set; }

    }
}