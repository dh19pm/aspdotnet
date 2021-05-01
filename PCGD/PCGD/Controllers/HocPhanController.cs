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
    public class HocPhanController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: HocPhan
        public ActionResult Index()
        {
            return View(db.HocPhan.OrderByDescending(x => x.ID).ToList());
        }

        // Post: HocPhan/Search
        [HttpPost]
        public JsonResult Search([Bind(Include = "MaHP")] HocPhan hocPhan)
        {
            return new JsonResult() { Data = db.HocPhan.Where(x => x.MaHP.Contains(hocPhan.MaHP)).Select(x => new
            {
                value = x.MaHP
            }).ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: HocPhan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HocPhan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaHP,TenHP,SoTC,SoTCBatBuoc,SoTCTuChon,SoTietLT,SoTietTH")] HocPhan hocPhan)
        {
            if (ModelState.IsValid)
            {
                if (db.HocPhan.Where(x => x.MaHP == hocPhan.MaHP).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại trên hệ thống!");
                    return View(hocPhan);
                }

                if (hocPhan.SoTCBatBuoc.HasValue && hocPhan.SoTCTuChon.HasValue)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Lỗi! Vui lòng không nhập cả trường số tính chỉ bắt buộc và tự chọn.");
                    return View(hocPhan);
                }

                if (!hocPhan.SoTCBatBuoc.HasValue && !hocPhan.SoTCTuChon.HasValue)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Lỗi! Vui lòng không bỏ trống cả trường số tính chỉ bắt buộc và tự chọn.");
                    return View(hocPhan);
                }

                if (!hocPhan.SoTietLT.HasValue && !hocPhan.SoTietTH.HasValue)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Lỗi! Vui lòng không bỏ trống cả trường số tiết lý thuyết và thực hành.");
                    return View(hocPhan);
                }

                db.HocPhan.Add(hocPhan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hocPhan);
        }

        // GET: HocPhan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocPhan hocPhan = db.HocPhan.Find(id);
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            return View(hocPhan);
        }

        // POST: HocPhan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaHP,TenHP,SoTC,SoTCBatBuoc,SoTCTuChon,SoTietLT,SoTietTH")] HocPhan hocPhan)
        {
            if (ModelState.IsValid)
            {
                if (db.HocPhan.Where(x => x.ID != hocPhan.ID && x.MaHP == hocPhan.MaHP).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại trên hệ thống!");
                    return View(hocPhan);
                }

                if (hocPhan.SoTCBatBuoc.HasValue && hocPhan.SoTCTuChon.HasValue)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Lỗi! Vui lòng không nhập cả trường số tính chỉ bắt buộc và tự chọn.");
                    return View(hocPhan);
                }

                if (!hocPhan.SoTCBatBuoc.HasValue && !hocPhan.SoTCTuChon.HasValue)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Lỗi! Vui lòng không bỏ trống cả trường số tính chỉ bắt buộc và tự chọn.");
                    return View(hocPhan);
                }

                if (!hocPhan.SoTietLT.HasValue && !hocPhan.SoTietTH.HasValue)
                {
                    ModelState.AddModelError("ThongBaoLoi", "Lỗi! Vui lòng không bỏ trống cả trường số tiết lý thuyết và thực hành.");
                    return View(hocPhan);
                }

                db.Entry(hocPhan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hocPhan);
        }

        // GET: HocPhan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocPhan hocPhan = db.HocPhan.Find(id);
            if (hocPhan == null)
            {
                return HttpNotFound();
            }
            return View(hocPhan);
        }

        // POST: HocPhan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HocPhan hocPhan = db.HocPhan.Find(id);
            db.HocPhan.Remove(hocPhan);
            db.SaveChanges();
            return RedirectToAction("Index");
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
