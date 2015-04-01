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
    
    public partial class Product
    {
        public Product()
        {
            this.OrderLines = new HashSet<OrderLine>();
            this.ProductBenefits = new HashSet<ProductBenefit>();
            this.ShoppingCarts = new HashSet<ShoppingCart>();
        }
    
        public int ProductID { get; set; }
        public string GLCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual ICollection<ProductBenefit> ProductBenefits { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}