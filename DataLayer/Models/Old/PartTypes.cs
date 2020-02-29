using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PartTypes :ItemDBHandler
    {
        public PartTypes(int? id=0)
           : base(id)
        {

        }
        public PartTypes()
        {
        }
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public override void SaveChildren()
        {
        }
    }
}
