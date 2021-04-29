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
    public class NganhController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: Nganh
        public ActionResult Index()
        {
            var nganh = db.Nganh.Include(n => n.Khoa);
            return View(nganh.ToList());
        }

        // GET: Nganh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganh.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            return View(nganh);
        }

        // GET: Nganh/Create
        public ActionResult Create()
        {
            ViewBag.Khoa_ID = new SelectList(db.Khoa, "ID", "TenKhoa");
            return View();
        }

        // POST: Nganh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Khoa_ID,TenNganh")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                db.Nganh.Add(nganh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Khoa_ID = new SelectList(db.Khoa, "ID", "TenKhoa", nganh.Khoa_ID);
            return View(nganh);
        }

        // GET: Nganh/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganh.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            ViewBag.Khoa_ID = new SelectList(db.Khoa, "ID", "TenKhoa", nganh.Khoa_ID);
            return View(nganh);
        }

        // POST: Nganh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Khoa_ID,TenNganh")] Nganh nganh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Khoa_ID = new SelectList(db.Khoa, "ID", "TenKhoa", nganh.Khoa_ID);
            return View(nganh);
        }

        // GET: Nganh/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganh.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            return View(nganh);
        }

        // POST: Nganh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nganh nganh = db.Nganh.Find(id);
            db.Nganh.Remove(nganh);
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
