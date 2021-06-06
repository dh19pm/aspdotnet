using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PCGD.Models;

namespace PCGD.Controllers
{
    [Authentication]
    public class LopController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: Lop
        [Role("Admin")]
        public ActionResult Index(string text = "", int page = 1)
        {
            var data = db.Lop.Include(l => l.ChuongTrinh).Include(l => l.Nganh);
            page = (page > 0 ? page : 1);
            int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaginationLimit"]);
            int start = (int)(page - 1) * pageSize;
            int totalPage = data.Where(x => x.TenLop.Contains(text) || text == "").Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            if (page <= 0 || (page > numSize && numSize > 0))
            {
                return HttpNotFound();
            }
            this.ViewBag.searchString = text;
            this.ViewBag.Page = page;
            this.ViewBag.Total = numSize;
            return View(data.OrderByDescending(x => x.ID).Skip(start).Where(x => x.TenLop.Contains(text) || text == "").Take(pageSize).ToList());
        }

        // GET: Lop/Create
        [Role("Admin")]
        public ActionResult Create()
        {
            ViewBag.ChuongTrinh_ID = new SelectList(db.ChuongTrinh, "ID", "TenCT");
            ViewBag.Nganh_ID = new SelectList(db.Nganh, "ID", "TenNganh");
            return View();
        }

        // POST: Lop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Role("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nganh_ID,ChuongTrinh_ID,TenLop,SoSV")] Lop lop)
        {
            if (ModelState.IsValid)
            {
                if (db.Lop.Where(x => x.TenLop == lop.TenLop).Count() > 0)
                {
                    ModelState.AddModelError("TenLop", "Tên lớp đã tồn tại trên hệ thống!");
                    return View(lop);
                }
                db.Lop.Add(lop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChuongTrinh_ID = new SelectList(db.ChuongTrinh, "ID", "TenCT", lop.ChuongTrinh_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganh, "ID", "TenNganh", lop.Nganh_ID);
            return View(lop);
        }

        // GET: Lop/Edit/5
        [Role("Admin")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lop lop = db.Lop.Find(id);
            if (lop == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChuongTrinh_ID = new SelectList(db.ChuongTrinh, "ID", "TenCT", lop.ChuongTrinh_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganh, "ID", "TenNganh", lop.Nganh_ID);
            return View(lop);
        }

        // POST: Lop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Role("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nganh_ID,ChuongTrinh_ID,TenLop,SoSV")] Lop lop)
        {
            if (ModelState.IsValid)
            {
                if (db.Lop.Where(x => x.TenLop == lop.TenLop && x.ID != lop.ID).Count() > 0)
                {
                    ModelState.AddModelError("TenLop", "Tên lớp đã tồn tại trên hệ thống!");
                    return View(lop);
                }
                db.Entry(lop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChuongTrinh_ID = new SelectList(db.ChuongTrinh, "ID", "TenCT", lop.ChuongTrinh_ID);
            ViewBag.Nganh_ID = new SelectList(db.Nganh, "ID", "TenNganh", lop.Nganh_ID);
            return View(lop);
        }

        // GET: Lop/Delete/5
        [Role("Admin")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lop lop = db.Lop.Find(id);
            if (lop == null)
            {
                return HttpNotFound();
            }
            return View(lop);
        }

        // POST: Lop/Delete/5
        [Role("Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Lop lop = db.Lop.Find(id);
            db.Lop.Remove(lop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Post: GiangVien/Search
        [Role("Admin", "User")]
        public JsonResult Search(string mahp, string tengv, string tenlop)
        {
            tengv = HttpUtility.UrlDecode(tengv);
            if (string.IsNullOrEmpty(mahp) && string.IsNullOrEmpty(tengv) && !string.IsNullOrEmpty(tenlop))
            {
                return new JsonResult()
                {
                    Data = db.Lop.Where(x => x.TenLop.Contains(tenlop)).Select(x => new { value = x.TenLop }).ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
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
                            where h.MaHP == mahp
                            select new { value = l.TenLop }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
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
                            where g.TenGV == tengv
                            select new { value = l.TenLop }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (!string.IsNullOrEmpty(mahp) && !string.IsNullOrEmpty(tengv) && string.IsNullOrEmpty(tenlop))
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
                            where h.MaHP == mahp && g.TenGV == tengv
                            select new { value = l.TenLop }).Distinct().ToList(),
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
                            where l.TenLop.Contains(tenlop) && h.MaHP == mahp
                            select new { value = l.TenLop }).Distinct().ToList(),
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
                            where l.TenLop.Contains(tenlop) && g.TenGV == tengv
                            select new { value = l.TenLop }).Distinct().ToList(),
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
                            where l.TenLop.Contains(tenlop) && h.MaHP == mahp && g.TenGV == tengv
                            select new { value = l.TenLop }).Distinct().ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = db.Lop.Select(x => new { value = x.TenLop }).ToList(),
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
