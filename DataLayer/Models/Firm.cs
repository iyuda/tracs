using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Firm : ItemDBHandler
    {
        public Firm(int id)
        : base(id)
        {
            //LoadRelations();
        }
        public Firm() { }
        public string FirmName { get; set; }
        public int? ContactId { get; set; }

        public List<REL_Bank_Firm> rel_Bank_Firms { get; set; }
        public User contact { get; set; }
        public static List<Firm> fullList
        { get;
            set;
        }
       
        public void LoadRelations()
        {
            LoadRelBankFirms();
            LoadContact();
        }
        public void LoadRelBankFirms()
        {
            Test_LoadRelBankFirms(); return;
            parameters.Clear();
            parameters.Add("id", new object[] { this.id.ToString(), SqlDbType.Int });
            List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            rel_Bank_Firms = DataHelper.GetQueryListBySP<REL_Bank_Firm>("REL_Bank_FirmGetByFirmId", sqlParameters);
            foreach (REL_Bank_Firm rel_Bank_Firm in rel_Bank_Firms)
            {
                rel_Bank_Firm.LoadRelations();
            }
            
        }
        public void LoadContact()
        {
            return;
            parameters.Clear();
            parameters.Add("id", new object[] { this.ContactId.ToString(), SqlDbType.Int });
            List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            var result = DataHelper.GetQueryListBySP<User>("UserGetById", sqlParameters);
            if (result.Count > 0)
            {
                this.contact = result[0];
            }
        }

        private void Test_LoadRelBankFirms()
        {
            rel_Bank_Firms = new List<REL_Bank_Firm>() { new REL_Bank_Firm(1) { CompanyId = 1, FirmId = this.id  }, new REL_Bank_Firm(2) { CompanyId = 2, FirmId = this.id} };
            foreach (REL_Bank_Firm rel_Bank_Firm in rel_Bank_Firms)
            {
                rel_Bank_Firm.LoadRelations();
            }


        }
    }
}