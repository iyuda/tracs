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
    
    public partial class Part
    {
        public int id { get; set; }
        public string SN { get; set; }
        public string PartNo { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> rma_details_id { get; set; }
    
        public virtual RMADetail RMADetail { get; set; }
    }
}
