using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SurveySiteInspection : ItemDBHandler
    {
        public SurveySiteInspection(int? id=0)
           : base(id)
        {

        }
        public SurveySiteInspection()
        {
        }
        
        public int? survey_id { get; set; }
        public string reader1_serial { get; set; }
        public string reader1_image_path { get; set; }
        public string reader1_image_description { get; set; }
        public string reader2_serial { get; set; }
        public string reader2_image_path { get; set; }
        public string reader2_image_description { get; set; }
        public string controller_serial { get; set; }
        public string controller_image_path { get; set; }
        public string controller_image_description { get; set; }
        public string existing_firm_version { get; set; }
        public string ups_present { get; set; }
        public string ups_installed { get; set; }
        public string new_firm_version { get; set; }
        public string card_reader_wiring { get; set; }
        public string card_reader_wiring_image_desc { get; set; }
        public string card_reader_wiring_image_path { get; set; }
        public string physical_int_devices { get; set; }
        public string elecrtrical_int_devices { get; set; }
        public string is_mounted { get; set; }
        public string supply_needed { get; set; }

        public override void SaveChildren()
        {
        }
       
    }
}
