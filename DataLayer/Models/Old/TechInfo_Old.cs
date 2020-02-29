using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class TechInfo_old:ItemDBHandler
    {
        public TechInfo_old(int? id=0)
           : base(id)
        {

        }
        public TechInfo_old() { }

        public string _ProblemDescription { get; set; }

        public string TechName { get; set;}
        public string TechEmail  { get; set; }
        public string TechPhone { get; set; }
        public string ClientName { get; set; }
        public string ClientComplaint { get; set; }
        public string ImageFile { get; set; }
        public string FieldObservation { get; set; }
        public string StepsUndertaken { get; set; }
        public string ComplaintConfirmed { get; set; }
        public override void SaveChildren()
        {
        }
    }
}
