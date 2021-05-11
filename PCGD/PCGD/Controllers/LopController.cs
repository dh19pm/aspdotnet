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
    [Role("Admin")]
    public class LopController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: Lop
        public ActionResult Index()
        {
            var lop = db.Lop.Include(l => l.ChuongTrinh).Include(l => l.Nganh);
            return View(lop.OrderByDescending(x => x.ID).ToList());
        }

        // GET: Lop/Create
        public ActionResult Create()
        {
            ViewBag.ChuongTrinh_ID = new SelectList(db.ChuongTrinh, "ID", "TenCT");
            ViewBag.Nganh_ID = new SelectList(db.Nganh, "ID", "TenNganh");
            return View();
        }

        // POST: Lop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Lop lop = db.Lop.Find(id);
            db.Lop.Remove(lop);
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
