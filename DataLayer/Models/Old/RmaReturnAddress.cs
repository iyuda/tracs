using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RmaReturnAddress : ItemDBHandler
    {
        public RmaReturnAddress(int? id=0)
           : base(id)
        {

        }
        public RmaReturnAddress()
        {
        }
        public string _ReturnAddressStateFullName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public override void SaveChildren()
        {
        }
    }
}
