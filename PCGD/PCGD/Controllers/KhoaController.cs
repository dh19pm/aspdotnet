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
    public class KhoaController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: Khoa
        public ActionResult Index(string text = "", int page = 1)
        {
            var data = db.Khoa;
            page = (page > 0 ? page : 1);
            int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaginationLimit"]);
            int start = (int)(page - 1) * pageSize;
            int totalPage = data.Where(x => x.TenKhoa.Contains(text) || text == "").Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            if (page <= 0 || (page > numSize && numSize > 0))
            {
                return HttpNotFound();
            }
            this.ViewBag.searchString = text;
            this.ViewBag.Page = page;
            this.ViewBag.Total = numSize;
            return View(data.OrderByDescending(x => x.ID).Skip(start).Where(x => x.TenKhoa.Contains(text) || text == "").Take(pageSize).ToList());
        }

        // GET: Khoa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Khoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenKhoa")] Khoa khoa)
        {
            if (ModelState.IsValid)
            {
                if (db.Khoa.Where(x => x.TenKhoa == khoa.TenKhoa).Count() > 0)
                {
                    ModelState.AddModelError("TenKhoa", "Tên khoa đã tồn tại trên hệ thống!");
                    return View(khoa);
                }
                db.Khoa.Add(khoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khoa);
        }

        // GET: Khoa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Khoa khoa = db.Khoa.Find(id);
            if (khoa == null)
            {
                return HttpNotFound();
            }
            return View(khoa);
        }

        // POST: Khoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenKhoa")] Khoa khoa)
        {
            if (ModelState.IsValid)
            {
                if (db.Khoa.Where(x => x.TenKhoa == khoa.TenKhoa && x.ID != khoa.ID).Count() > 0)
                {
                    ModelState.AddModelError("TenKhoa", "Tên khoa đã tồn tại trên hệ thống!");
                    return View(khoa);
                }
                db.Entry(khoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khoa);
        }

        // GET: Khoa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Khoa khoa = db.Khoa.Find(id);
            if (khoa == null)
            {
                return HttpNotFound();
            }
            return View(khoa);
        }

        // POST: Khoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Khoa khoa = db.Khoa.Find(id);
            db.Khoa.Remove(khoa);
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
