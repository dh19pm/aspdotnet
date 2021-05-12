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
    }
}