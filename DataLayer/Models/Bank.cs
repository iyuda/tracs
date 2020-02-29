using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Bank : ItemDBHandler
    {
        public Bank(int id)
        : base(id)
        {
            LoadRelations();
        }
        public Bank() { }

        public static List<Bank> fullList { get; set; }

        [Display(Name = "Name")]
        [Required, StringLength(50)]
        public String Name { get; set; }

        [Display(Name = "LongName")]
        [Required, StringLength(100)]
        public String LongName { get; set; }

        public List<CompanyBranch> companyBranches { get; set; }
        public void LoadRelations()
        {
            LoadBranches();
        }

        public void LoadBranches()
        {
            // test area
                companyBranches = Test_GetBranches();
                return;

            

        }
        

        private List<CompanyBranch> Test_GetBranches()
        {

            return new List<CompanyBranch>
            {
                new CompanyBranch{
                    id = 1,
                    CompanyId = (int) this.id,
                    FullAddress = "FullAddress 01",
                    Address = "Address 01",
                    City = "City 01",
                    CompanyBranchStateId=1,
                    ZipCode="11111"
                },
                new CompanyBranch{
                    id = 2,
                    CompanyId = (int) this.id,
                    FullAddress = "FullAddress 02",
                    Address = "Address 02",
                    City = "City 02",
                    CompanyBranchStateId=2,
                    ZipCode="11111"
                }
            };
            
        }

    }
}