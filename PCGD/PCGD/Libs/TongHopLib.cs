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
        public static double GetTongGioDay(long TongHopID, long GiangVienID)
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
                        where t.ID == TongHopID && g.ID == GiangVienID
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
        public static List<TongHopModel> GetTongHopModel(long TongHopID)
        {
            PCGDEntities db = new PCGDEntities();
            return (from c in db.ChiTietTongHop
                    join g in db.GiangVien on c.GiangVien_ID equals g.ID
                    where c.TongHop_ID == TongHopID
                    orderby g.ID ascending
                    select new TongHopModel
                    {
                        ID = c.ID,
                        GiangVien_ID = g.ID,
                        TenGV = g.TenGV,
                        DinhMucGioChuan = c.DinhMucGioChuan,
                        DinhMucCongTac = c.DinhMucCongTac,
                        GiamDinhMuc = c.GiamDinhMuc,
                        GhiChu = c.GhiChu
                    }).ToList();
        }
        public static long GetChiTietTongHopID(long TongHopID,long GiangVienID)
        {
            PCGDEntities db = new PCGDEntities();
            ChiTietTongHop chiTietTongHop = db.ChiTietTongHop.Where(x => x.TongHop_ID == TongHopID && x.GiangVien_ID == GiangVienID).SingleOrDefault();
            return (chiTietTongHop == null ? 0 : chiTietTongHop.ID);
        }
    }
}