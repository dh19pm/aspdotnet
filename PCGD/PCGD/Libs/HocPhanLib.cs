using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCGD.Models;

namespace PCGD.Libs
{
    public class HocPhanLib
    {
        public static string ConvertHocKi(int num)
        {
            switch (num)
            {
                case 1:
                    return "I";
                case 2:
                    return "II";
                case 3:
                    return "III";
                case 4:
                    return "IV";
                case 5:
                    return "V";
                case 6:
                    return "VI";
                case 7:
                    return "VII";
                default:
                    return "VIII";
            }
        }
        public static int getCountHocPhanTuChon(long nhomHocPhanID)
        {
            PCGDEntities db = new PCGDEntities();
            return (from n in db.NhomHocPhan
                    join c in db.ChiTietHocPhan on n.ID equals c.NhomHocPhan_ID
                    join h in db.HocPhan on c.HocPhan_ID equals h.ID
                    where n.ID == nhomHocPhanID && h.LoaiHP == 1
                    select new { }).Count();
        }
        public static int getCountHocPhanBatBuoc(long nhomHocPhanID)
        {
            PCGDEntities db = new PCGDEntities();
            return (from n in db.NhomHocPhan
                    join c in db.ChiTietHocPhan on n.ID equals c.NhomHocPhan_ID
                    join h in db.HocPhan on c.HocPhan_ID equals h.ID
                    where n.ID == nhomHocPhanID && h.LoaiHP == 0
                    select new { }).Count();
        }
        public static int getCountHocPhan(long nhomHocPhanID)
        {
            PCGDEntities db = new PCGDEntities();
            return (from n in db.NhomHocPhan
                    join c in db.ChiTietHocPhan on n.ID equals c.NhomHocPhan_ID
                    join h in db.HocPhan on c.HocPhan_ID equals h.ID
                    where n.ID == nhomHocPhanID
                    select new { }).Count();
        }
        public static List<HocPhanModel> getHocPhanModel(long nhomHocPhanID)
        {
            PCGDEntities db = new PCGDEntities();
            return (from c in db.ChiTietHocPhan
                    join h in db.HocPhan on c.HocPhan_ID equals h.ID
                    join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                    where n.ID == nhomHocPhanID
                    orderby c.ID ascending
                    select new HocPhanModel
                    {
                        ID = h.ID,
                        ChiTietHocPhan_ID = c.ID,
                        MaHP = h.MaHP,
                        TenHP = h.TenHP,
                        LoaiHP = h.LoaiHP,
                        HocPhanDieuKien = n.HocPhanDieuKien,
                        SoTC = h.SoTC,
                        SoTietLT = c.SoTietLT,
                        SoTietTH = c.SoTietTH
                    }).ToList();
        }
        public static List<ViewHocKiModel> getViewHocKiModel()
        {
            PCGDEntities db = new PCGDEntities();
            List<ViewHocKiModel> viewHocKiModel = new List<ViewHocKiModel>();
            var modelHocKi = db.HocKi;
            foreach (var hocKi in modelHocKi)
            {
                ViewHocKiModel itemHocKi = new ViewHocKiModel();
                itemHocKi.ID = hocKi.ID;
                itemHocKi.ChuongTrinh_ID = hocKi.ChuongTrinh_ID;
                itemHocKi.SoHocKi = hocKi.SoHocKi;
                int tongHP = 0, tongTC = 0, tongTCBatBuoc = 0, tongTCTuChon = 0;
                var modelNhomHocPhan = db.NhomHocPhan.Where(x => x.HocKi_ID == hocKi.ID).ToList();
                foreach (var nhomHocPhan in modelNhomHocPhan)
                {
                    int soHP = 0;
                    var modelChiTietHocPhan = db.ChiTietHocPhan.Where(x => x.NhomHocPhan_ID == nhomHocPhan.ID).ToList();
                    foreach (var chiTietHocPhan in modelChiTietHocPhan)
                    {
                        var hocPhan = db.HocPhan.Where(x => x.ID == chiTietHocPhan.HocPhan_ID).SingleOrDefault();
                        if (hocPhan != null)
                        {
                            if (nhomHocPhan.HocPhanDieuKien == 0 && nhomHocPhan.HocPhanThayThe == 0)
                            {
                                if (hocPhan.LoaiHP == 0)
                                {
                                    tongTCBatBuoc = tongTCBatBuoc + hocPhan.SoTC;
                                }
                                else if (hocPhan.LoaiHP == 1)
                                {
                                    if (modelChiTietHocPhan.Count() == 1)
                                        tongTCTuChon = tongTCTuChon + hocPhan.SoTC;
                                    else
                                        if (soHP == 0)
                                        tongTCTuChon = tongTCTuChon + nhomHocPhan.TongTC;
                                }
                            }
                            soHP++;
                        }
                    }
                    if (nhomHocPhan.HocPhanThayThe == 1)
                    {
                        soHP++;
                    }
                    tongHP = tongHP + soHP;
                    tongTC = tongTCBatBuoc + tongTCTuChon;
                }
                itemHocKi.SoHocPhan = tongHP;
                itemHocKi.TongTC = tongTC;
                itemHocKi.TongTCBatBuoc = tongTCBatBuoc;
                itemHocKi.TongTCTuChon = tongTCTuChon;
                viewHocKiModel.Add(itemHocKi);
            }
            return viewHocKiModel;
        }
        public static bool existMaHocPhan(long nhomHocPhanID, string MaHP, long chiTietHocPhanID = 0)
        {
            PCGDEntities db = new PCGDEntities();
            int countRow = 0;

            if (chiTietHocPhanID > 0)
                countRow = (from n in db.NhomHocPhan
                            join c in db.ChiTietHocPhan on n.ID equals c.NhomHocPhan_ID
                            join h in db.HocPhan on c.HocPhan_ID equals h.ID
                            where n.ID == nhomHocPhanID && h.MaHP == MaHP && c.ID != chiTietHocPhanID
                            select new { }).Count();
            else
                countRow = (from n in db.NhomHocPhan
                            join c in db.ChiTietHocPhan on n.ID equals c.NhomHocPhan_ID
                            join h in db.HocPhan on c.HocPhan_ID equals h.ID
                            where n.ID == nhomHocPhanID && h.MaHP == MaHP
                            select new { }).Count();

            if (countRow > 0)
                return true;

            return false;
        }
    }
}