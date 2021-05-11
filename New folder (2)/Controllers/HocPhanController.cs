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
        public ActionResult Create([Bind(Include = "ID,MaHP,LoaiHP,TenHP,SoTC")] HocPhan hocPhan)
        {
            if (ModelState.IsValid)
            {
                if (db.HocPhan.Where(x => x.MaHP == hocPhan.MaHP).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại trên hệ thống!");
                    return View(hocPhan);
                }

                db.HocPhan.Add(hocPhan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hocPhan);
        }

        // GET: HocPhan/Edit/5
        public ActionResult Edit(long? id)
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
        public ActionResult Edit([Bind(Include = "ID,MaHP,LoaiHP,TenHP,SoTC")] HocPhan hocPhan)
        {
            if (ModelState.IsValid)
            {
                if (db.HocPhan.Where(x => x.ID != hocPhan.ID && x.MaHP == hocPhan.MaHP).Count() > 0)
                {
                    ModelState.AddModelError("MaHP", "Mã học phần đã tồn tại trên hệ thống!");
                    return View(hocPhan);
                }

                db.Entry(hocPhan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hocPhan);
        }

        // GET: HocPhan/Delete/5
        public ActionResult Delete(long? id)
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
        public ActionResult DeleteConfirmed(long id)
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
