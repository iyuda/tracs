using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PuttyTest 
        : ItemDBHandler
    {
        public PuttyTest(int? id=0)
           : base(id)
        {

        }
        public PuttyTest() { }
        public string VerifyComms { get; set;}
        public string MagneticCardReaderTest { get; set; }
        public string NFCProximityTest { get; set; }
        public string BeaconFunctional { get; set; }
        public string Notes { get; set; }
        public override void SaveChildren()
        {
        }
    }
}
