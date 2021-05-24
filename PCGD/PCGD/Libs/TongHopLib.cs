using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCGD.Models;

namespace PCGD.Libs
{
    public class TongHopLib
    {
        public static double GetTongTietCuaTungHocKi(long TongHopID, byte HocKi, long GiangVienID)
        {
            PCGDEntities db = new PCGDEntities();
            var data = (from t in db.TongHop
                        join p in db.PhanCong on t.ID equals p.TongHop_ID
                        join n in db.NhiemVu on p.ID equals n.PhanCong_ID
                        join g in db.GiangVien on n.GiangVien_ID equals g.ID
                        join h in db.HocPhan on n.HocPhan_ID equals h.ID
                        join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                        join o in db.NhomHocPhan on c.NhomHocPhan_ID equals o.ID
                        join k in db.HocKi on o.HocKi_ID equals k.ID
                        join l in db.Lop on n.Lop_ID equals l.ID
                        where t.ID == TongHopID && p.HocKi == HocKi && g.ID == GiangVienID
                        orderby c.ID ascending
                        select new
                        {
                            ID = g.ID,
                            SoSV = l.SoSV,
                            SoTietLT = c.SoTietLT,
                            SoTietTH = c.SoTietTH,
                            NhomLT = n.NhomLT,
                            NhomTH = n.NhomTH,
                            TongTiet = 0
                        }).AsEnumerable().Select(x => new
                        {
                            ID = x.ID,
                            TongTiet = PhanCongLib.TinhTongTiet(PhanCongLib.TinhTongTietLT((x.SoTietLT == null ? 0 : (int)x.SoTietLT), (x.NhomLT == null ? 0 : (int)x.NhomLT), PhanCongLib.TinhHeSo(x.SoSV)), PhanCongLib.TinhTongTietTH((x.SoTietTH == null ? 0 : (int)x.SoTietTH), (x.NhomTH == null ? 0 : (int)x.NhomTH)))
                        }).GroupBy(x => new { x.ID }).Select(x => new { TongTiet = x.Sum(s => s.TongTiet) }).SingleOrDefault();
            return data == null ? 0.0 : data.TongTiet;
        }
        public static List<TongHopModel> GetTongHopModel(long? id)
        {
            PCGDEntities db = new PCGDEntities();
            return (from t in db.TongHop
                    join p in db.PhanCong on t.ID equals p.TongHop_ID
                    join n in db.NhiemVu on p.ID equals n.PhanCong_ID
                    join g in db.GiangVien on n.GiangVien_ID equals g.ID
                    join i in db.ChiTietTongHop on g.ID equals i.GiangVien_ID
                    join h in db.HocPhan on n.HocPhan_ID equals h.ID
                    join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                    join o in db.NhomHocPhan on c.NhomHocPhan_ID equals o.ID
                    join k in db.HocKi on o.HocKi_ID equals k.ID
                    join l in db.Lop on n.Lop_ID equals l.ID
                    where t.ID == id
                    orderby c.ID ascending
                    select new
                    {
                        ID = i.ID,
                        TongHop_ID = t.ID,
                        GiangVien_ID = g.ID,
                        TenGV = g.TenGV,
                        DinhMucGioChuan = i.DinhMucGioChuan,
                        DinhMucCongTac = i.DinhMucCongTac,
                        GiamDinhMuc = i.GiamDinhMuc,
                        GhiChu = i.GhiChu,
                        SoSV = l.SoSV,
                        SoTietLT = c.SoTietLT,
                        SoTietTH = c.SoTietTH,
                        NhomLT = n.NhomLT,
                        NhomTH = n.NhomTH,
                        HocKi1 = 0,
                        HocKi2 = 0,
                        TongTiet = 0
                    }).AsEnumerable().Select(x => new TongHopModel
                    {
                        ID = x.ID,
                        TenGV = x.TenGV,
                        DinhMucGioChuan = x.DinhMucGioChuan,
                        DinhMucCongTac = x.DinhMucCongTac,
                        GiamDinhMuc = x.GiamDinhMuc,
                        GhiChu = x.GhiChu,
                        HocKi1 = GetTongTietCuaTungHocKi(x.TongHop_ID, 1, x.GiangVien_ID),
                        HocKi2 = GetTongTietCuaTungHocKi(x.TongHop_ID, 2, x.GiangVien_ID),
                        TongTiet = PhanCongLib.TinhTongTiet(PhanCongLib.TinhTongTietLT((x.SoTietLT == null ? 0 : (int)x.SoTietLT), (x.NhomLT == null ? 0 : (int)x.NhomLT), PhanCongLib.TinhHeSo(x.SoSV)), PhanCongLib.TinhTongTietTH((x.SoTietTH == null ? 0 : (int)x.SoTietTH), (x.NhomTH == null ? 0 : (int)x.NhomTH)))
                    }).GroupBy(x => new {
                        x.ID,
                        x.TenGV,
                        x.DinhMucGioChuan,
                        x.DinhMucCongTac,
                        x.GiamDinhMuc,
                        x.GhiChu,
                        x.HocKi1,
                        x.HocKi2
                    }).Select(x => new TongHopModel
                    {
                        ID = x.Key.ID,
                        TenGV = x.Key.TenGV,
                        DinhMucGioChuan = x.Key.DinhMucGioChuan,
                        DinhMucCongTac = x.Key.DinhMucCongTac,
                        GiamDinhMuc = x.Key.GiamDinhMuc,
                        HocKi1 = x.Key.HocKi1,
                        HocKi2 = x.Key.HocKi2,
                        GhiChu = x.Key.GhiChu,
                        TongTiet = x.Sum(s => s.TongTiet)
                    }).ToList();
        }
    }
}