using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LogMyWork.Models;
using Microsoft.AspNet.Identity;
using LogMyWork.DTO.Rates;

namespace LogMyWork.Controllers
{
    [Authorize]
    public class RatesController : Controller, IRatesController
    {
        private LogMyWorkContext db = new LogMyWorkContext();

        // GET: Rates
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var rates = db.Rates.Include(r => r.User).Where(r => r.User.Id == userID);
            return View(rates.ToList());
        }


        // GET: Rates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RateCreateDTO dto)
        {
            Rate rate = new Rate();
            rate.RateValue = dto.RateValue;
            if (ModelState.IsValid)
            {
                rate.UserID = User.Identity.GetUserId();

                db.Rates.Add(rate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {
                return View(rate);
            }
        }



        // GET: Rates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            if (rate.UserID == User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            else
                return View(rate);
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rate rate = db.Rates.Find(id);
            if(rate.UserID == User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.Rates.Remove(rate);
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
