using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class CompanyBranch : ItemDBHandler
    {
        public CompanyBranch(int id)
        : base(id)
        {

        }
        public CompanyBranch() { }

        [Display(Name = "FullAddress")]
        [Required, StringLength(255)]
        [JsonProperty]
        public String FullAddress { get; set; }

        [Display(Name = "Address")]
        [Required, StringLength(255)]
        [DataMember(Name = "StreetAddress")]
        [JsonProperty(PropertyName = "StreetAddress")]
        public String Address { get; set; }

        [Display(Name = "City")]
        [Required, StringLength(50)]
        [DataMember]
        [JsonProperty]
        public String City { get; set; }

        [Display(Name = "StateId")]
        [Required]
        [DataMember]
        [JsonProperty(PropertyName = "StateId")]
        public int? CompanyBranchStateId { get; set; }

        [Display(Name = "CountryId")]
        [DataMember]
        [JsonProperty(PropertyName = "CountryId")]
        public int? CompanyBranchCountryId { get; set; }

        [Display(Name = "ZipCode")]
        [Required, StringLength(10)]
        [DataMember(Name = "Zip")]
        [JsonProperty(PropertyName = "Zip")]
        public String ZipCode { get; set; }

        [Display(Name = "CompanyId")]
        [Required]
        [DataMember]
        [JsonProperty]
        public int? CompanyId { get; set; }

        [Display(Name = "UserId")]
        [Required]
        [DataMember]
        [JsonProperty(PropertyName = "UserId")]
        public int? CompanyAddressUserId { get; set; }

        [JsonIgnore]
        public State state { get; set; }

        [JsonIgnore]
        public Bank bank { get; set; }

        public void LoadState()
        {
            parameters.Clear();
            parameters.Add("id", new object[] { this.CompanyBranchStateId.ToString(), SqlDbType.Int });
            List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            var result = DataHelper.GetQueryListBySP<State>("StateGetById", sqlParameters);
            if (result.Count > 0)
            {
                this.state = result[0];
            }
        }
        public void LoadBank()
        {
            // test area

            Test_LoadBank();
            return;
        }
        private void Test_LoadBank()
        {
            this.bank = Bank.fullList.FirstOrDefault(x => x.id == this.CompanyId);

        }

    }
}