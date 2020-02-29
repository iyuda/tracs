using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class BankInfo : ItemDBHandler
    {
        public BankInfo(int? id=0)
           : base(id)
        {

        }
        public BankInfo()
        {
        }
        public string _BankStateFullName { get; set; }
        public string BankName { get; set;}
        public string BankStreetAddress { get; set; }
        public string BankCity { get; set; }
        public string BankState { get; set; }
        public string BankZipCode { get; set; }
        public override void SaveChildren()
        {
        }
    }
}
