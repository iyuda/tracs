using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SurveyIntegrityChecks : ItemDBHandler
    {
        public SurveyIntegrityChecks(int? id=0)
           : base(id)
        {

        }
        public SurveyIntegrityChecks()
        {
        }
        // Property names that start with underscore are are the ones that do not have corresponding database fields
        public string _message { get; set; }
        public int? survey_id { get; set; }
        public int? seq_no { get; set; }
        public string integrity_check { get; set; }


        public override void SaveChildren()
        {
        }
       
    }
}
