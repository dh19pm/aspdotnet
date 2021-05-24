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
    public class NganhController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: Nganh
        public ActionResult Index(string text = "", int page = 1)
        {
            var data = db.Nganh.Include(n => n.Khoa);
            page = (page > 0 ? page : 1);
            int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaginationLimit"]);
            int start = (int)(page - 1) * pageSize;
            int totalPage = data.Where(x => x.TenNganh.Contains(text) || text == "").Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            if (page <= 0 || (page > numSize && numSize > 0))
            {
                return HttpNotFound();
            }
            this.ViewBag.searchString = text;
            this.ViewBag.Page = page;
            this.ViewBag.Total = numSize;
            return View(data.OrderByDescending(x => x.ID).Skip(start).Where(x => x.TenNganh.Contains(text) || text == "").Take(pageSize).ToList());
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
                if (db.Nganh.Where(x => x.TenNganh == nganh.TenNganh).Count() > 0)
                {
                    ModelState.AddModelError("TenNganh", "Tên ngành đã tồn tại trên hệ thống!");
                    return View(nganh);
                }
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
                if (db.Nganh.Where(x => x.TenNganh == nganh.TenNganh && x.ID != nganh.ID).Count() > 0)
                {
                    ModelState.AddModelError("TenNganh", "Tên ngành đã tồn tại trên hệ thống!");
                    return View(nganh);
                }
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
