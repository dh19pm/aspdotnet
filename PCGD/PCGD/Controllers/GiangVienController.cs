using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PCGD.Models;
using PCGD.Libs;

namespace PCGD.Controllers
{
    [Authentication]
    [Role("Admin")]
    public class GiangVienController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: GiangVien
        public ActionResult Index(string text = "", int page = 1)
        {
            var data = db.GiangVien;
            page = (page > 0 ? page : 1);
            int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaginationLimit"]);
            int start = (int)(page - 1) * pageSize;
            int totalPage = data.Where(x => x.TenGV.Contains(text) || text == "").Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            if (page <= 0 || (page > numSize && numSize > 0))
            {
                return HttpNotFound();
            }
            this.ViewBag.searchString = text;
            this.ViewBag.Page = page;
            this.ViewBag.Total = numSize;
            return View(data.OrderByDescending(x => x.ID).Skip(start).Where(x => x.TenGV.Contains(text) || text == "").Take(pageSize).ToList());
        }

        // GET: GiangVien/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangVien.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            this.ViewBag.TenGV = giangVien.TenGV;
            ViewGiangVienModel viewGiangVienModel = new ViewGiangVienModel();
            viewGiangVienModel.GiangVien_ID = giangVien.ID;
            viewGiangVienModel.HocPhan = HocPhanLib.GetHocPhanGiangVienModels(giangVien.ID);
            return View(viewGiangVienModel);
        }

        // GET: GiangVien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GiangVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenGV")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                if (db.GiangVien.Where(x => x.TenGV == giangVien.TenGV).Count() > 0)
                {
                    ModelState.AddModelError("TenGV", "Tên giảng viên đã tồn tại trên hệ thống!");
                    return View(giangVien);
                }

                db.GiangVien.Add(giangVien);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = giangVien.ID });
            }

            return View(giangVien);
        }

        // GET: GiangVien/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangVien.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            return View(giangVien);
        }

        // POST: GiangVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenGV")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                if (db.GiangVien.Where(x => x.TenGV == giangVien.TenGV && x.ID != giangVien.ID).Count() > 0)
                {
                    ModelState.AddModelError("TenGV", "Tên giảng viên đã tồn tại trên hệ thống!");
                    return View(giangVien);
                }

                db.Entry(giangVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(giangVien);
        }

        // GET: GiangVien/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangVien.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            return View(giangVien);
        }

        // POST: GiangVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            GiangVien giangVien = db.GiangVien.Find(id);
            db.GiangVien.Remove(giangVien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: GiangVien/Create
        public ActionResult ThemHocPhan(long? id)
        {
            GiangVien giangVien = db.GiangVien.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            this.ViewBag.TenGV = giangVien.TenGV;
            ThemHocPhanGiangVienModel themHocPhanGiangVienModel = new ThemHocPhanGiangVienModel();
            themHocPhanGiangVienModel.GiangVien_ID = giangVien.ID;
            return View(themHocPhanGiangVienModel);
        }

        // POST: GiangVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHocPhan([Bind(Include = "GiangVien_ID,MaHP")] ThemHocPhanGiangVienModel themHocPhanGiangVienModel)
        {
            if (ModelState.IsValid)
            {
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == themHocPhanGiangVienModel.MaHP).SingleOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(themHocPhanGiangVienModel);
                }
                if (db.ChiTietGiangVien.Where(x => x.GiangVien_ID == themHocPhanGiangVienModel.GiangVien_ID && x.HocPhan_ID == hocPhan.ID).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại!");
                    return View(themHocPhanGiangVienModel);
                }
                ChiTietGiangVien chiTietGiangVien = new ChiTietGiangVien();
                chiTietGiangVien.GiangVien_ID = themHocPhanGiangVienModel.GiangVien_ID;
                chiTietGiangVien.HocPhan_ID = hocPhan.ID;
                db.ChiTietGiangVien.Add(chiTietGiangVien);
                db.SaveChanges();
                return RedirectToAction("Details", "GiangVien", new { id = themHocPhanGiangVienModel.GiangVien_ID });
            }
            GiangVien giangVien = db.GiangVien.Find(themHocPhanGiangVienModel.GiangVien_ID);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            this.ViewBag.TenGV = giangVien.TenGV;
            return View(themHocPhanGiangVienModel);
        }

        // GET: GiangVien/Edit/5
        public ActionResult SuaHocPhan(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietGiangVien chiTietGiangVien = db.ChiTietGiangVien.Find(id);
            if (chiTietGiangVien == null)
            {
                return HttpNotFound();
            }
            HocPhan hocPhan = db.HocPhan.Where(x => x.ID == chiTietGiangVien.HocPhan_ID).SingleOrDefault();
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            this.ViewBag.TenGV = chiTietGiangVien.GiangVien.TenGV;
            SuaHocPhanGiangVienModel suaHocPhanGiangVienModel = new SuaHocPhanGiangVienModel();
            suaHocPhanGiangVienModel.ChiTietGiangVien_ID = chiTietGiangVien.ID;
            suaHocPhanGiangVienModel.MaHP = hocPhan.MaHP;
            suaHocPhanGiangVienModel.GiangVien_ID = chiTietGiangVien.GiangVien_ID;
            return View(suaHocPhanGiangVienModel);
        }

        // POST: GiangVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaHocPhan([Bind(Include = "ChiTietGiangVien_ID,GiangVien_ID,MaHP")] SuaHocPhanGiangVienModel suaHocPhanGiangVienModel)
        {
            if (ModelState.IsValid)
            {
                ChiTietGiangVien chiTietGiangVien = db.ChiTietGiangVien.Find(suaHocPhanGiangVienModel.ChiTietGiangVien_ID);
                if (chiTietGiangVien == null)
                {
                    return HttpNotFound();
                }
                HocPhan hocPhan = db.HocPhan.Where(x => x.MaHP == suaHocPhanGiangVienModel.MaHP).SingleOrDefault();
                if (hocPhan == null)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần không tồn tại trên hệ thống!");
                    return View(suaHocPhanGiangVienModel);
                }
                if (db.ChiTietGiangVien.Where(x => x.ID != chiTietGiangVien.ID && x.HocPhan_ID == hocPhan.ID).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại!");
                    return View(suaHocPhanGiangVienModel);
                }
                chiTietGiangVien.HocPhan_ID = hocPhan.ID;
                db.Entry(chiTietGiangVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "GiangVien", new { id = chiTietGiangVien.GiangVien_ID });
            }
            GiangVien giangVien = db.GiangVien.Find(suaHocPhanGiangVienModel.GiangVien_ID);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            this.ViewBag.TenGV = giangVien.TenGV;
            return View(suaHocPhanGiangVienModel);
        }

        // GET: GiangVien/XoaHocPhan/5
        public ActionResult XoaHocPhan(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietGiangVien chiTietGiangVien = db.ChiTietGiangVien.Find(id);
            if (chiTietGiangVien == null)
            {
                return HttpNotFound();
            }
            HocPhan hocPhan = db.HocPhan.Where(x => x.ID == chiTietGiangVien.HocPhan_ID).SingleOrDefault();
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            this.ViewBag.TenGV = chiTietGiangVien.GiangVien.TenGV;
            XoaHocPhanGiangVienModel xoaHocPhanGiangVienModel = new XoaHocPhanGiangVienModel();
            xoaHocPhanGiangVienModel.ChiTietGiangVien_ID = chiTietGiangVien.ID;
            xoaHocPhanGiangVienModel.MaHP = hocPhan.MaHP;
            xoaHocPhanGiangVienModel.GiangVien_ID = chiTietGiangVien.GiangVien_ID;
            return View(xoaHocPhanGiangVienModel);
        }

        // POST: GiangVien/XoaHocPhan/5
        [HttpPost, ActionName("XoaHocPhan")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaHocPhanConfirmed(long? id)
        {
            ChiTietGiangVien chiTietGiangVien = db.ChiTietGiangVien.Find(id);
            db.ChiTietGiangVien.Remove(chiTietGiangVien);
            db.SaveChanges();
            return RedirectToAction("Details", "GiangVien", new { id = chiTietGiangVien.GiangVien_ID });
        }

        // Post: GiangVien/Search
        public JsonResult Search(string mahp, string tengv, string tenlop)
        {
            tengv = HttpUtility.UrlDecode(tengv);
            if (string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = db.GiangVien.Where(x => x.TenGV.Contains(tengv)).Select(x => new { value = x.TenGV }).ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from g in db.GiangVien
                            join c in db.ChiTietGiangVien on g.ID equals c.GiangVien_ID
                            join h in db.HocPhan on c.HocPhan_ID equals h.ID
                            where h.MaHP == mahp
                            select new { value = g.TenGV }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (string.IsNullOrEmpty(mahp) && string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from h in db.HocPhan
                            join v in db.ChiTietGiangVien on h.ID equals v.HocPhan_ID
                            join g in db.GiangVien on v.GiangVien_ID equals g.ID
                            join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                            join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                            join k in db.HocKi on n.HocKi_ID equals k.ID
                            join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                            join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                            where l.TenLop == tenlop
                            select new { value = g.TenGV }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from g in db.GiangVien
                            join c in db.ChiTietGiangVien on g.ID equals c.GiangVien_ID
                            join h in db.HocPhan on c.HocPhan_ID equals h.ID
                            where g.TenGV.Contains(tengv) && h.MaHP == mahp
                            select new { value = g.TenGV }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from h in db.HocPhan
                            join v in db.ChiTietGiangVien on h.ID equals v.HocPhan_ID
                            join g in db.GiangVien on v.GiangVien_ID equals g.ID
                            join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                            join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                            join k in db.HocKi on n.HocKi_ID equals k.ID
                            join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                            join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                            where h.MaHP == mahp && l.TenLop == tenlop
                            select new { value = g.TenGV }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from h in db.HocPhan
                            join v in db.ChiTietGiangVien on h.ID equals v.HocPhan_ID
                            join g in db.GiangVien on v.GiangVien_ID equals g.ID
                            join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                            join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                            join k in db.HocKi on n.HocKi_ID equals k.ID
                            join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                            join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                            where g.TenGV.Contains(tengv) && l.TenLop == tenlop
                            select new { value = g.TenGV }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = (from h in db.HocPhan
                            join v in db.ChiTietGiangVien on h.ID equals v.HocPhan_ID
                            join g in db.GiangVien on v.GiangVien_ID equals g.ID
                            join c in db.ChiTietHocPhan on h.ID equals c.HocPhan_ID
                            join n in db.NhomHocPhan on c.NhomHocPhan_ID equals n.ID
                            join k in db.HocKi on n.HocKi_ID equals k.ID
                            join t in db.ChuongTrinh on k.ChuongTrinh_ID equals t.ID
                            join l in db.Lop on t.ID equals l.ChuongTrinh_ID
                            where g.TenGV.Contains(tengv) && h.MaHP == mahp && l.TenLop == tenlop
                            select new { value = g.TenGV }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = db.GiangVien.Select(x => new { value = x.TenGV }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
