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
    
    public partial class Date
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Date()
        {
            this.RMAs = new HashSet<RMA>();
        }
    
        public int id { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public Nullable<System.DateTime> ShippingDate { get; set; }
        public Nullable<System.DateTime> MFGDate { get; set; }
        public Nullable<System.DateTime> CaseDate { get; set; }
        public Nullable<System.DateTime> TestDate { get; set; }
        public Nullable<System.DateTime> ReworkDate { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RMA> RMAs { get; set; }
    }
}