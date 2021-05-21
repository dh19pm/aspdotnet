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
    public class TongHopController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: TongHop
        public ActionResult Index()
        {
            return View(db.TongHop.ToList());
        }

        // GET: TongHop/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TongHop tongHop = db.TongHop.Find(id);
            if (tongHop == null)
            {
                return HttpNotFound();
            }
            return View(tongHop);
        }

        // GET: TongHop/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TongHop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NamHoc")] TongHop tongHop)
        {
            if (ModelState.IsValid)
            {
                db.TongHop.Add(tongHop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tongHop);
        }

        // GET: TongHop/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TongHop tongHop = db.TongHop.Find(id);
            if (tongHop == null)
            {
                return HttpNotFound();
            }
            return View(tongHop);
        }

        // POST: TongHop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NamHoc")] TongHop tongHop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tongHop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tongHop);
        }

        // GET: TongHop/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TongHop tongHop = db.TongHop.Find(id);
            if (tongHop == null)
            {
                return HttpNotFound();
            }
            return View(tongHop);
        }

        // POST: TongHop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TongHop tongHop = db.TongHop.Find(id);
            db.TongHop.Remove(tongHop);
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
