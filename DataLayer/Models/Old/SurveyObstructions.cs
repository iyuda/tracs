using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SurveyObstructions : ItemDBHandler
    {
        public SurveyObstructions(int? id=0)
           : base(id)
        {

        }
        public SurveyObstructions()
        {
        }
        public string _message { get; set; }
        public int? survey_id { get; set; }
        public int? seq_no { get; set; }
        public string yes_no { get; set; }
        public string description { get; set; }
        public string is_picture_taken { get; set; }
        public string picture_path { get; set; }


        public override void SaveChildren()
        {
        }
       
    }
}
