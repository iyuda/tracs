using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RmaBase : ItemDBHandler
    {
        public RmaBase(int id)
           : base(id)
        {

        }
        public RmaBase() { }

        // Property names that start with underscore are are the ones that do not have corresponding database fields
        public string _form_name { get; set; }
        public string _form_title { get; set; }
        public string _email_address { get; set; }
        public string _email_name { get; set; }
        public string _status { get; set; }

        public string tr_no { get; set; }
        public string case_no { get; set; }
        public bool? was_parabit_called { get; set; }
        public string dispatch_no { get; set; }

        public int? status_id { get; set; }
        public int? securitas_options_id { get; set; }
        public int? bankinfo_id { get; set; }
        public int? assessment_id { get; set; }
        public int? dates_id { get; set; }
        public int? putty_test_id { get; set; }
        public int? tech_info_id { get; set; }
        public int? test_results_id { get; set; }
        public int? return_address_id { get; set; }
        public int? form_id { get; set; }
        public int? problem_id { get; set; }

        public Boolean UpdateStatus(int? status=null)
        {
            try
            {
                string strConnect;

                if (status == null) status = this.status_id;
                strConnect = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                using (SqlConnection cn = new SqlConnection(strConnect))
                {
                    cn.Open();
                    string UpdateString = "update RmaBase set status_id = '" + status + "' where id = '" + this.id + "'";

                    SqlCommand cmd = new SqlCommand(UpdateString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                        return false;
                    return rows > 0;
                }

            }
            catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        
        public override void SaveChildren()
        {

            //RmaSecuritasOptions RMADetail = new RmaSecuritasOptions(securitas_options_id);
            //if (RMADetail.Save()) this.securitas_options_id = RMADetail.id;

            //BankInfo BankInfo = new BankInfo(bankinfo_id);
            //if (BankInfo.Save()) this.bankinfo_id = BankInfo.id;

            //Assessment Assessment = new Assessment(assessment_id);
            //if (Assessment.Save()) this.assessment_id = Assessment.id;

            //RmaDates RmaDates = new RmaDates(dates_id);
            //if (RmaDates.Save()) this.dates_id = RmaDates.id;

            //PuttyTest PuttyTest = new PuttyTest(putty_test_id);
            //if (PuttyTest.Save()) this.putty_test_id = PuttyTest.id;

            //TechInfo TechInfo = new TechInfo(tech_info_id);
            //if (TechInfo.Save()) this.tech_info_id = TechInfo.id;

            //RmaTests RmaTests = new RmaTests(test_results_id);
            //if (RmaTests.Save()) this.test_results_id = RmaTests.id;

            //RmaReturnAddress RmaReturnAddress = new RmaReturnAddress(return_address_id);
            //if (RmaReturnAddress.Save()) this.return_address_id = RmaReturnAddress.id;

           
        }
    }
}
