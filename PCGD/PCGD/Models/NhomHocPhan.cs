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
    
    public partial class NhomHocPhan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhomHocPhan()
        {
            this.ChiTietHocPhan = new HashSet<ChiTietHocPhan>();
        }
    
        public long ID { get; set; }
        public long HocKi_ID { get; set; }
        public byte HocPhanDieuKien { get; set; }
        public byte HocPhanThayThe { get; set; }
        public int TongTC { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHocPhan> ChiTietHocPhan { get; set; }
        public virtual HocKi HocKi { get; set; }
    }
}
