//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PCGD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChiTietHocPhan
    {
        public long ID { get; set; }
        public long NhomHocPhan_ID { get; set; }
        public long HocPhan_ID { get; set; }
        public Nullable<int> SoTietLT { get; set; }
        public Nullable<int> SoTietTH { get; set; }
    
        public virtual HocPhan HocPhan { get; set; }
        public virtual NhomHocPhan NhomHocPhan { get; set; }
    }
}
