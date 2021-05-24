﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;

    public partial class TongHop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TongHop()
        {
            this.ChiTietTongHop = new HashSet<ChiTietTongHop>();
            this.PhanCong = new HashSet<PhanCong>();
        }
    
        public long ID { get; set; }

        [Display(Name = "Năm học")]
        public int NamHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTongHop> ChiTietTongHop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhanCong> PhanCong { get; set; }
    }
    public class TongHopModel
    {
        public long ID { get; set; }

        [Display(Name = "Tên giảng viên")]
        public string TenGV { get; set; }

        [Display(Name = "ĐM Giờ chuẩn")]
        public Nullable<int> DinhMucGioChuan { get; set; }

        [Display(Name = "ĐM Công tác")]
        public Nullable<int> DinhMucCongTac { get; set; }

        [Display(Name = "Giảm ĐM")]
        public Nullable<double> GiamDinhMuc { get; set; }

        [Display(Name = "Học kì I")]
        public double HocKi1 { get; set; }

        [Display(Name = "Học kì II")]
        public double HocKi2 { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        [Display(Name = "Tổng giờ dạy")]
        public double TongTiet { get; set; }
    }
    public partial class ChiTietTongHopModel
    {
        public long ID { get; set; }

        [Display(Name = "Định mức giờ chuẩn")]
        public Nullable<int> DinhMucGioChuan { get; set; }

        [Display(Name = "Định mức công tác")]
        public Nullable<int> DinhMucCongTac { get; set; }

        [Display(Name = "Giảm định mức")]
        public Nullable<double> GiamDinhMuc { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
    }
}
