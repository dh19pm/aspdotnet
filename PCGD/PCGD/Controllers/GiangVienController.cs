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
    public class GiangVienController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: GiangVien
        public ActionResult Index()
        {
            return View(db.GiangVien.ToList());
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
            return View(giangVien);
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
                db.GiangVien.Add(giangVien);
                db.SaveChanges();
                return RedirectToAction("Index");
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
