using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Part : ItemDBHandler
    {
        public Part(int id)
        : base(id)
        {

        }
        public Part() { }
        
        public string SerialNumber { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ReworkDate { get; set; }
        public int? PartTypeId { get; set; }

        public static List<Part> fullList { get; set; }
        
        
    }
}