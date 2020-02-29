using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RmaTests : ItemDBHandler
    {
        public RmaTests(int? id=0)
           : base(id)
        {

        }
        public RmaTests() { }
        public string TestResults { get; set;}
        public string FactoryTech { get; set; }

        public override void SaveChildren()
        {
        }
    }
}
