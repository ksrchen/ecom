//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ecom.web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserBenefit
    {
        public int UserBenefitID { get; set; }
        public int UserID { get; set; }
        public int BenefitID { get; set; }
        public Nullable<int> FromOrderLineID { get; set; }
    
        public virtual Benefit Benefit { get; set; }
        public virtual User User { get; set; }
    }
}
