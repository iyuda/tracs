//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RMATracker.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RMA
    {
        public int id { get; set; }
        public string received { get; set; }
        public string rma_no { get; set; }
        public string tr_no { get; set; }
        public Nullable<int> case_no { get; set; }
        public Nullable<System.DateTime> date_submitted { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public Nullable<int> rma_details_id { get; set; }
        public Nullable<int> bankinfo_id { get; set; }
        public Nullable<int> assessment_id { get; set; }
        public Nullable<int> dates_id { get; set; }
        public Nullable<int> putty_test_id { get; set; }
        public Nullable<int> tech_info_id { get; set; }
        public Nullable<int> test_results_id { get; set; }
        public Nullable<int> return_address_id { get; set; }
        public string DispatchNo { get; set; }
    
        public virtual BankInfo BankInfo { get; set; }
        public virtual Date Date { get; set; }
        public virtual PuttyTest PuttyTest { get; set; }
        public virtual ReturnAddress ReturnAddress { get; set; }
        public virtual RMADetail RMADetail { get; set; }
        public virtual TechInfo TechInfo { get; set; }
        public virtual Test Test { get; set; }
    }
}