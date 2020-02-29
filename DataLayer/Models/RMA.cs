using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [DataContract]
    public class RMA : ItemDBHandler
    {
        public RMA(int id)
        : base(id)
        {

        }
        public RMA() {  }
        public RMA(string Token) { LoadRelations(Token); }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [DataMember] public string TrackingNumber { get; set; }
        [DataMember] public string ServiceCallDate { get; set; }
        [DataMember] public int? RMATypeId { get; set; }
        [DataMember] public int? CreditReasonId { get; set; }
        
        [DataMember] public int? ReturnAddressId { get; set; }
        [DataMember] public int? ParabitCallId { get; set; }
        [DataMember] public int? CompanyBranchId { get; set; }
        [DataMember]

        [Required(ErrorMessage = "my custom error message")]
        public int? CompanyId { get; set; }
        [DataMember] public int? UserId { get; set; }
        [DataMember] public int? FirmId { get; set; }
        [DataMember] public DispatchReason dispatchReason { get; set; }
        [DataMember] public List<REL_RMA_Fault> rel_RMA_Faults { get; set; }
        [DataMember] public List<ReturnedPart> returnedParts { get; set; }
        [DataMember] public List<RMAMedia> RMAMedias { get; set; }
        [DataMember] public ParabitCall parabitCall { get; set; }
        [DataMember] public string Token { get; set; }

        public int? DispatchReasonId { get; set; }
        public int? TechnicianId { get; set; }
        public bool? IsCorrectShipping { get; set; }
        

        public RMAType rmaType { get; set; }
        public CompanyBranch companyBranch { get; set; }
        public CreditReason creditReason { get; set; }
        public REL_User_Attribute technician { get; set; }
        public REL_RMA_Status rel_rma_status { get; set; }
        public REL_RMA_Fault rel_rma_fault { get; set; }
        public ReturnAddress returnAddress { get; set; }
        
        
        
        
        


        public void LoadRelations(string Token="")
        {
            //LoadRMAType(Token);
            //LoadReturnedParts();
            //LoadPictures();
           // LoadTechnician();
        }
        //public void LoadRMAType(string Token)
        //{
        //    this.rmaType = UrlRequests.GetRMATypes(Token);
        //}

            public void LoadReturnedParts()
        {
            // test area
            //companyBranches = Test_GetBranches();
            Test_LoadReturnedParts();
            return;

            //parameters.Clear();
            //parameters.Add("id", new object[] { this.id.ToString(), SqlDbType.Int });
            //List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            //companyBranches = DataHelper.GetQueryListBySP<CompanyBranch>("ReturnedPartsGetByRMAId", sqlParameters);


        }
       
        public void Test_LoadReturnedParts()
        {
            this.returnedParts = new List<ReturnedPart>();
            this.returnedParts.Add(new ReturnedPart());
            this.returnedParts[0].PartId = 1;
            this.returnedParts[0].LoadRelations();
            this.returnedParts.Add(new ReturnedPart());
            this.returnedParts[1].PartId = 2;
            this.returnedParts[1].LoadRelations();

        }
        public void LoadPictures()
        {
            // test area

            Test_LoadPictures();
            return;
        }
        private void Test_LoadPictures()
        {
            this.RMAMedias = new List<RMAMedia>();
            this.RMAMedias.Add(new RMAMedia() { id = 1, RMAId = 1 });
            this.RMAMedias.Add(new RMAMedia() { id = 2, RMAId = 1 });
        }
        public void LoadTechnician()
        {
            // test area

            Test_LoadTechnician();
            return;
        }
        private void Test_LoadTechnician()
        {
            this.technician = new REL_User_Attribute() { id = this.TechnicianId };
            
        }
        public void LoadCompanyBranch()
        {
            // test area

            Test_LoadCompanyBranch();
            return;
        }
        private void Test_LoadCompanyBranch()
        {
            this.companyBranch = new CompanyBranch() { id = this.CompanyBranchId };
            companyBranch.LoadBank();
        }
    }
}