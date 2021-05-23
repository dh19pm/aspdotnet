using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCGD.Models;

namespace PCGD.Libs
{
    public class TongHopLib
    {
        public static double GetSoGioHocKi(long PhanCongID, byte HocKi, long GiangVienID)
        {
            var Data = PhanCongLib.GetNhiemVuModel(PhanCongID).Where(w => w.HocKi == HocKi && w.GiangVien_ID == GiangVienID).GroupBy(g => new { g.GiangVien_ID }).Select(e => new { ID = e.Key.GiangVien_ID, SoGioDay = e.Sum(s => s.TongTiet) }).SingleOrDefault();
            if (Data != null)
                return Data.SoGioDay;
            return 0.0;
        }
        public static List<TongHopModel> GetTongHopModel(long? id)
        {
            PCGDEntities db = new PCGDEntities();
            return (from t in db.TongHop
                    join p in db.PhanCong on t.ID equals p.TongHop_ID
                    join c in db.ChiTietTongHop on t.ID equals c.TongHop_ID
                    join g in db.GiangVien on c.GiangVien_ID equals g.ID
                    where t.ID == id
                    select new TongHopModel
                    {
                        PhanCong_ID = p.ID,
                        GiangVien_ID = g.ID,
                        TenGV = g.TenGV,
                        DinhMucGioChuan = c.DinhMucGioChuan,
                        DinhMucCongTac = c.DinhMucCongTac,
                        GiamDinhMuc = c.GiamDinhMuc,
                        HocKi1 = 0,
                        HocKi2 = 0,
                        GhiChu = c.GhiChu
                    }).AsEnumerable().Select(x => new TongHopModel {
                        PhanCong_ID = x.PhanCong_ID,
                        GiangVien_ID = x.GiangVien_ID,
                        TenGV = x.TenGV,
                        DinhMucGioChuan = x.DinhMucGioChuan,
                        DinhMucCongTac = x.DinhMucCongTac,
                        GiamDinhMuc = x.GiamDinhMuc,
                        HocKi1 = GetSoGioHocKi(x.PhanCong_ID, 1, x.GiangVien_ID),
                        HocKi2 = GetSoGioHocKi(x.PhanCong_ID, 2, x.GiangVien_ID),
                        GhiChu = x.GhiChu
                    }).Distinct().ToList();
        }
    }
}