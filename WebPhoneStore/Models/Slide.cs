//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebPhoneStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Slide
    {
        public long ID { get; set; }
        public string Image { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifileDate { get; set; }
        public string ModifileBy { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
