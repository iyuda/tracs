using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RmaProblemTypes : ItemDBHandler
    {
        public RmaProblemTypes(int? id = 0)
           : base(id)
        {

        }
        public RmaProblemTypes()
        {
        }

        public string problem_description { get; set; }
        
        public override void SaveChildren()
        {
        }
    }
}
