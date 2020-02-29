using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RmaStatuses : ItemDBHandler
    {

        public RmaStatuses()
        {
        }

        public string status { get; set; }
        
        public override void SaveChildren()
        {
        }
    }
}
