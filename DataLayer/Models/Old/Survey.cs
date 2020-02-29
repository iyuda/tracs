using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Survey : ItemDBHandler
    {
        public Survey(int id)
           : base(id)
        {

        }
        public Survey() {
  
        }


        public string _SurveyStateFullName { get; set; }
        public string _CableOriginFullName { get; set; }
        public List<bool> _visit_reqs { get; set; }

  
        public string survey_date { get; set; }
        public string tech_name { get; set; }
        public string requestor { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string requester_problem { get; set; }
        public string problem_reporter { get; set; }
        public string phone { get; set; }
        public string contact_problem { get; set; }
        public string heartbeat_main { get; set; }
        public string heartbeat_rs485 { get; set; }
        public string alarm_tamper { get; set; }
        public string alarm_overlay { get; set; }
        public string alarm_cable_cut { get; set; }
        public string alarm_relay1 { get; set; }
        public string alarm_relay2 { get; set; }
        public string alarm_relay3 { get; set; }
        public string heartbeat_main_other { get; set; }
        public string heartbeat_rs485_other { get; set; }
        public string alarm_tamper_other { get; set; }
        public string alarm_overlay_other { get; set; }
        public string alarm_cable_cut_other { get; set; }
        public string alarm_relay1_other { get; set; }
        public string alarm_relay2_other { get; set; }
        public string alarm_relay3_other { get; set; }
        public string client_able_connect { get; set; }
        public string retrieve_ip_completed { get; set; }
        public string trail_file_name1 { get; set; }
        public string trail_file_name2 { get; set; }
        public string client_contact { get; set; }
        public string integrity_check6_image_path { get; set; }
        public string pair1_ohms { get; set; }
        public string pair2_ohms { get; set; }
        public string pair3_ohms { get; set; }
        public string pair4_ohms { get; set; }
        public string cable_origin { get; set; }
        public string cable_gauge { get; set; }
        public string cable_gauge_other { get; set; }
        public string has_twisted_pairs { get; set; }
        public string replacement_needed { get; set; }
        public string new_cable_installed { get; set; }
        public string visit_reqs { get; set; }
        public string is_cable_picture_taken { get; set; }
        public string cable_picture_path { get; set; }
        public string unique_reqs { get; set; }
        public string overlay_activity { get; set; }
        //public string obstructions_found { get; set; }
        //public string obstructions_found_notes { get; set; }
        //public string is_obstr_picture_taken { get; set; }
        //public string obstr_picture_path { get; set; }
        //public string obstructions_reported { get; set; }
        //public string obstructions_reported_notes { get; set; }
        public string additional_findings { get; set; }
        public string new_serial_reader1 { get; set; }
        public string new_serial_reader2 { get; set; }
        public string tamper_observations { get; set; }
        public string continuity_test_ok { get; set; }
        public string changed_switch1 { get; set; }
        public string joint_show_damage { get; set; }
        public string changed_switch2 { get; set; }
        public string replaced_mmr_reader { get; set; }
        public string replaced_collar { get; set; }
        public string inspection_results { get; set; }
        public string inspection_picture_taken { get; set; }
        public string inspection_picture_path { get; set; }
        public string serial_shunted { get; set; }
        public string serial_shunted_results { get; set; }
        public string branch_staff_name { get; set; }


   
        
        public override void SaveChildren()
        {

        }
    }
}
