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

    public partial class HocPhan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HocPhan()
        {
            this.ChiTietGiangVien = new HashSet<ChiTietGiangVien>();
            this.ChiTietHocPhan = new HashSet<ChiTietHocPhan>();
            this.NhiemVu = new HashSet<NhiemVu>();
        }
    
        public long ID { get; set; }

        [Display(Name = "Mã học phần")]
        [Required(ErrorMessage = "Mã học phần không được bỏ trống!")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã học phần buộc phải là 6 kí tự!")]
        public string MaHP { get; set; }

        [Display(Name = "Tên học phần")]
        [Required(ErrorMessage = "Tên học phần không được bỏ trống!")]
        public string TenHP { get; set; }

        [Display(Name = "Loại học phần")]
        [Required(ErrorMessage = "Loại học phần không được bỏ trống!")]
        public byte LoaiHP { get; set; }

        [Display(Name = "Số tín chỉ")]
        [Required(ErrorMessage = "Số tín chỉ học phần không được bỏ trống!")]
        [Range(1, 10, ErrorMessage = "Số tín chỉ phải nằm trong khoảng 1 - 10!")]
        public int SoTC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietGiangVien> ChiTietGiangVien { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHocPhan> ChiTietHocPhan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhiemVu> NhiemVu { get; set; }
    }
}