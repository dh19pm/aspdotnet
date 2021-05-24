using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PCGD.Models;
using PCGD.Libs;

namespace PCGD.Controllers
{
    [Authentication]
    [Role("Admin", "User")]
    public class TongHopController : Controller
    {
        private PCGDEntities db = new PCGDEntities();

        // GET: TongHop
        public ActionResult Index(string text = "", int page = 1)
        {
            var data = db.TongHop;
            page = (page > 0 ? page : 1);
            int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaginationLimit"]);
            int start = (int)(page - 1) * pageSize;
            int totalPage = data.Where(x => x.NamHoc.ToString().Contains(text) || (x.NamHoc + 1).ToString().Contains(text) || text == "").Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            if (page <= 0 || (page > numSize && numSize > 0))
            {
                return HttpNotFound();
            }
            this.ViewBag.searchString = text;
            this.ViewBag.Page = page;
            this.ViewBag.Total = numSize;
            return View(data.OrderByDescending(x => x.NamHoc).Skip(start).Where(x => x.NamHoc.ToString().Contains(text) || (x.NamHoc + 1).ToString().Contains(text) || text == "").Take(pageSize).ToList());
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
            this.ViewBag.NamHoc = tongHop.NamHoc;
            List<TongHopModel> tongHopModel = TongHopLib.GetTongHopModel(id);
            return View(tongHopModel);
        }

        // GET: TongHop/SuaGiangVien/5
        public ActionResult SuaGiangVien(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietTongHop chiTietTongHop = db.ChiTietTongHop.Find(id);
            if (chiTietTongHop == null)
            {
                return HttpNotFound();
            }
            this.ViewBag.TenGV = chiTietTongHop.GiangVien.TenGV;
            this.ViewBag.NamHoc = chiTietTongHop.TongHop.NamHoc;
            this.ViewBag.TongHop_ID = chiTietTongHop.TongHop_ID;
            ChiTietTongHopModel chiTietTongHopModel = new ChiTietTongHopModel();
            chiTietTongHopModel.ID = chiTietTongHop.ID;
            chiTietTongHopModel.DinhMucCongTac = chiTietTongHop.DinhMucCongTac;
            chiTietTongHopModel.DinhMucGioChuan = chiTietTongHop.DinhMucGioChuan;
            chiTietTongHopModel.GiamDinhMuc = chiTietTongHop.GiamDinhMuc;
            chiTietTongHopModel.GhiChu = chiTietTongHop.GhiChu;
            return View(chiTietTongHopModel);
        }

        // POST: TongHop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaGiangVien([Bind(Include = "ID,DinhMucCongTac,DinhMucGioChuan,GiamDinhMuc,GhiChu")] ChiTietTongHopModel chiTietTongHopModel)
        {
            ChiTietTongHop chiTietTongHop = db.ChiTietTongHop.Find(chiTietTongHopModel.ID);
            if (chiTietTongHop == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                chiTietTongHop.DinhMucGioChuan = chiTietTongHopModel.DinhMucGioChuan;
                chiTietTongHop.DinhMucCongTac = chiTietTongHopModel.DinhMucCongTac;
                chiTietTongHop.GiamDinhMuc = chiTietTongHopModel.GiamDinhMuc;
                chiTietTongHop.GhiChu = chiTietTongHopModel.GhiChu;
                db.Entry(chiTietTongHop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = chiTietTongHop.TongHop_ID });
            }
            this.ViewBag.TenGV = chiTietTongHop.GiangVien.TenGV;
            this.ViewBag.NamHoc = chiTietTongHop.TongHop.NamHoc;
            this.ViewBag.TongHop_ID = chiTietTongHop.TongHop_ID;
            return View(chiTietTongHopModel);
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
