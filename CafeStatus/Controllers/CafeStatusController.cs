using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CafeStatus.Models;
using PushSharp.Google;
using PushSharp.Core;

namespace CafeStatus.Controllers
{
    public class CafeStatusController : Controller
    {
        private CafeDBContext db = new CafeDBContext();

        // GET: CafeStatus
        public ActionResult Index()
        {
            return View(db.Status.OrderByDescending(p => p.Data).Take(10).ToList());
        }

        // GET: CafeStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CafeStatusModels cafeStatus = db.Status.Find(id);
            if (cafeStatus == null)
            {
                return HttpNotFound();
            }
            return View(cafeStatus);
        }

        // GET: CafeStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CafeStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(CafeStatusModels cafeStatus)
        {
            if (ModelState.IsValid)
            {
                var lastStatus = db.Status.OrderByDescending(p => p.Data).FirstOrDefault();
                if (cafeStatus.Pronto && (lastStatus == null || lastStatus.Pronto != cafeStatus.Pronto) )
                {
                    PushService.Send();
                }

                cafeStatus.Data = DateTime.Now;
                db.Status.Add(cafeStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cafeStatus);
        }


       
        // GET: CafeStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CafeStatusModels cafeStatus = db.Status.Find(id);
            if (cafeStatus == null)
            {
                return HttpNotFound();
            }
            return View(cafeStatus);
        }

        // POST: CafeStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoCafeStatus,Data,Pronto,Observacao")] CafeStatusModels cafeStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cafeStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cafeStatus);
        }

        // GET: CafeStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CafeStatusModels cafeStatus = db.Status.Find(id);
            if (cafeStatus == null)
            {
                return HttpNotFound();
            }
            return View(cafeStatus);
        }

        // POST: CafeStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CafeStatusModels cafeStatus = db.Status.Find(id);
            db.Status.Remove(cafeStatus);
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
