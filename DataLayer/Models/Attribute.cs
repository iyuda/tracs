using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Attribute : ItemDBHandler
    {
        public Attribute(int id)
        : base(id)
        {

        }
        public Attribute() { }


        public string Name { get; set; }

        public static List<Attribute> attributes
        {
            get
            {
                return new List<Attribute>() { new Attribute(1) { Name = "Internal Tech (\"Parabit\")" },
                                               new Attribute(2) { Name = "External Tech (\"Securitas\")" } };
            }
        }
       

}
}