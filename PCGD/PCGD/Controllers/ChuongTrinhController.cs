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
    public class ChuongTrinhController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: ChuongTrinh
        public ActionResult Index()
        {
            return View(db.ChuongTrinh.ToList());
        }

        // GET: ChuongTrinh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            if (chuongTrinh == null)
            {
                return HttpNotFound();
            }
            return View(chuongTrinh);
        }

        // GET: ChuongTrinh/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChuongTrinh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NgayTao")] ChuongTrinh chuongTrinh)
        {
            if (ModelState.IsValid)
            {
                db.ChuongTrinh.Add(chuongTrinh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chuongTrinh);
        }

        // GET: ChuongTrinh/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            if (chuongTrinh == null)
            {
                return HttpNotFound();
            }
            return View(chuongTrinh);
        }

        // POST: ChuongTrinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NgayTao")] ChuongTrinh chuongTrinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuongTrinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chuongTrinh);
        }

        // GET: ChuongTrinh/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            if (chuongTrinh == null)
            {
                return HttpNotFound();
            }
            return View(chuongTrinh);
        }

        // POST: ChuongTrinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChuongTrinh chuongTrinh = db.ChuongTrinh.Find(id);
            db.ChuongTrinh.Remove(chuongTrinh);
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
