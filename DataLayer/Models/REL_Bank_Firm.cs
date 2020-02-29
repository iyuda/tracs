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
    public class REL_Bank_Firm : ItemDBHandler
    {
        public REL_Bank_Firm(int id)
        : base(id)
        {

        }
        public REL_Bank_Firm() { }
        public int? CompanyId { get; set; }
        public int? FirmId { get; set; }

        public Firm firm { get; set; }
        public Bank bank { get; set; }

        public void LoadRelations()
        {
            LoadFirm();
            LoadBank();
        }
        public void LoadFirm()
        {

            parameters.Clear();
            parameters.Add("id", new object[] { this.FirmId.ToString(), SqlDbType.Int });
            List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            var result = DataHelper.GetQueryListBySP<Firm>("FirmGetById", sqlParameters);
            if (result.Count > 0)
            {
                this.firm = result[0];
            }
        }
        public void LoadBank()
        {
            this.bank = Test_LoadBank();return;
            parameters.Clear();
            parameters.Add("id", new object[] { this.CompanyId.ToString(), SqlDbType.Int });
            List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            var result = DataHelper.GetQueryListBySP<Bank>("BankGetById", sqlParameters);
            if (result.Count > 0)
            {
                this.bank = result[0];
            }
        }
        private Bank Test_LoadBank()
        {
            return Bank.fullList.FirstOrDefault(x => x.id == this.CompanyId);
        }

    }
}