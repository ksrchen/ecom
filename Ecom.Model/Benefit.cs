//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecom.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Benefit
    {
        public Benefit()
        {
            this.ProductBenefits = new HashSet<ProductBenefit>();
            this.UserBenefits = new HashSet<UserBenefit>();
        }
    
        public int BenefitID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<ProductBenefit> ProductBenefits { get; set; }
        public virtual ICollection<UserBenefit> UserBenefits { get; set; }
    }
}
