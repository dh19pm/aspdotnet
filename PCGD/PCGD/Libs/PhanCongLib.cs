using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCGD.Models;

namespace PCGD.Libs
{
    public class PhanCongLib
    {
        public static bool IsHocPhanOfGiangVien(string TenGV, string MaHP)
        {
            PCGDEntities db = new PCGDEntities();
            if ((from g in db.GiangVien
                 join c in db.ChiTietGiangVien on g.ID equals c.GiangVien_ID
                 join h in db.HocPhan on c.HocPhan_ID equals h.ID
                 where g.TenGV == TenGV && h.MaHP == MaHP
                 select new
                 {
                 }).Count() > 0)
                return true;
            return false;
        }
        public static bool IsLopOfHocPhan(string MaHP, string TenLop)
        {
            PCGDEntities db = new PCGDEntities();
            if ((from h in db.HocPhan
                 join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                 join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                 join k in db.HocKi on n.HocKi_ID equals k.ID
                 join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                 join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                 where h.MaHP == MaHP && l.TenLop == TenLop
                 select new { }).Count() > 0)
                return true;
            return false;
        }
        public static List<NhiemVuLopModel> GetNhiemVuLopModel(long PhanCongID)
        {
            PCGDEntities db = new PCGDEntities();
            return (from n in db.NhiemVu
                    join l in db.Lop on n.Lop_ID equals l.ID
                    join g in db.Nganh on l.Nganh_ID equals g.ID
                    where n.PhanCong_ID == PhanCongID
                    orderby l.TenLop ascending
                    select new NhiemVuLopModel
                    {
                        ID = l.ID,
                        TenLop = l.TenLop,
                        TenNganh = g.TenNganh
                    }).Distinct().ToList();
        }
        public static List<NhiemVuNhomHocPhanModel> GetNhiemVuNhomHocPhanModel(long PhanCongID)
        {
            PCGDEntities db = new PCGDEntities();
            return (from n in db.NhiemVu
                    join g in db.GiangVien on n.GiangVien_ID equals g.ID
                    join h in db.HocPhan on n.HocPhan_ID equals h.ID
                    join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                    join o in db.NhomHocPhan on c.NhomHocPhan_ID equals o.ID
                    join k in db.HocKi on o.HocKi_ID equals k.ID
                    join l in db.Lop on n.Lop_ID equals l.ID
                    where n.PhanCong_ID == PhanCongID
                    orderby o.ID ascending, k.SoHocKi ascending 
                    select new NhiemVuNhomHocPhanModel
                    {
                        ID = o.ID,
                        Lop_ID = l.ID,
                        HocPhanDieuKien = o.HocPhanDieuKien,
                        TongTC = o.TongTC
                    }).Distinct().ToList();
        }
        public static List<NhiemVuModel> GetNhiemVuModel(long PhanCongID)
        {
            PCGDEntities db = new PCGDEntities();
            return (from n in db.NhiemVu
                    join g in db.GiangVien on n.GiangVien_ID equals g.ID
                    join h in db.HocPhan on n.HocPhan_ID equals h.ID
                    join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                    join o in db.NhomHocPhan on c.NhomHocPhan_ID equals o.ID
                    join k in db.HocKi on o.HocKi_ID equals k.ID
                    join l in db.Lop on n.Lop_ID equals l.ID
                    where n.PhanCong_ID == PhanCongID
                    orderby c.ID ascending
                    select new NhiemVuModel
                    {
                        ID = n.ID,
                        Lop_ID = l.ID,
                        NhomHocPhan_ID = o.ID,
                        TenLop = l.TenLop,
                        SoSV = l.SoSV,
                        MaHP = h.MaHP,
                        TenHP = h.TenHP,
                        LoaiHP = h.LoaiHP,
                        SoTC = h.SoTC,
                        SoTietLT = c.SoTietLT,
                        SoTietTH = c.SoTietTH,
                        TenGV = g.TenGV,
                        LoaiPhong = n.LoaiPhong,
                        NhomLT = n.NhomLT,
                        NhomHT = n.NhomHT,
                        GhiChu = n.GhiChu
                    }).ToList();
        }
    }
}