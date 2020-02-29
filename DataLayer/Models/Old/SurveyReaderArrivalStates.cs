using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SurveyReaderArrivalStates : ItemDBHandler
    {
        public SurveyReaderArrivalStates(int? id=0)
           : base(id)
        {

        }
        public SurveyReaderArrivalStates()
        {
        }
        
        public int? survey_id { get; set; }
        public int? seq_no { get; set; }
        public string reader_green { get; set; }
        public string reader_green_schedule { get; set; }
        public string reader_amber { get; set; }
        public string reader_amber_schedule { get; set; }
        public string reader_red { get; set; }
        public string reader_red_schedule { get; set; }
        public string reader_blinking_red { get; set; }
        public string reader_blinking_red_schedule { get; set; }
        public string reader_other { get; set; }
        public string reader_success1 { get; set; }
        public string reader_success2 { get; set; }
        public string reader_notes { get; set; }



        public override void SaveChildren()
        {
        }
       
    }
}
