﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCGD.Models;

namespace PCGD.Libs
{
    public class PhanCongLib
    {
        public static decimal? TinhHeSo(int sv)
        {
            return (decimal)(sv <= 40 ? 1.0 : (sv <= 70 ? 1.1 : (sv <= 100 ? 1.2 : (sv <= 130 ? 1.3 : 0.0))));
        }
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
            List<NhiemVuModel> nhiemVu = (from n in db.NhiemVu
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
                                              NhomTH = n.NhomTH,
                                              GhiChu = n.GhiChu,
                                              HeSo = 0,
                                              TongTietLT = 0,
                                              TongTietTH = 0,
                                              TongTiet = 0
                                          }).ToList();
            foreach (var item in nhiemVu)
            {
                item.NhomLT = (item.NhomLT == null ? 0 : item.NhomLT);
                item.NhomTH = (item.NhomTH == null ? 0 : item.NhomTH);
                item.HeSo = TinhHeSo(item.SoSV);
                item.TongTietLT = ((item.SoTietLT == null ? 0 : item.SoTietLT) * item.NhomLT * item.HeSo);
                item.TongTietTH = ((item.SoTietTH == null ? 0 : item.SoTietTH) * item.NhomTH);
                item.TongTiet = (item.TongTietLT + item.TongTietTH / 2 + item.TongTietTH / 5);
            }
            return nhiemVu;
        }
        public static bool ExistsNhiemVu(long PhanCongID, long LopID, long HocPhanID, long GiaoVienID)
        {
            PCGDEntities db = new PCGDEntities();
            if (db.NhiemVu.Where(x => x.PhanCong_ID == PhanCongID && x.Lop_ID == LopID && x.HocPhan_ID == HocPhanID && x.GiangVien_ID == GiaoVienID).Count() > 0)
                return true;
            return false;
        }
    }
}