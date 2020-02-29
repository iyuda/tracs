using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Assessment:ItemDBHandler
    {
        public Assessment(int? id=0)
           : base(id)
        {
           
        }
        public Assessment()
        {
        }

        public string VerifyReturn { get; set;}
        public string PhysicalDamage { get; set; }
        public string PowerUp { get; set; }
        public string VerifyLED { get; set; }
        public string PosSwitch { get; set; }
        public string CableCut { get; set; }
        public string SkimAlarm { get; set; }
        public string CardRead { get; set; }
        public string NFCProximity { get; set; }
        public string UnitPassFail { get; set; }
        public string RootCause { get; set; }
        public string HardwareRootCause { get; set; }
        public override void SaveChildren()
        {
        }
        //public void Save()
        //{
        //    this.SaveChildren();
        //    this.InsertUpdateAction(0);
        //}

        //public void SaveChildren()
        //{
        //    if (!string.IsNullOrEmpty(agency_id))
        //    {
        //        Agency agency = new Agency(agency_id);
        //        agency.HandleRecord();
        //    }
        //    if (!string.IsNullOrEmpty(neighborhood))
        //    {
        //        Neighborhood neighborhoodobj = new Neighborhood(neighborhood);
        //        neighborhoodobj.HandleRecord();
        //    }
        //}
    }
}
