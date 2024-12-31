using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeManagementWaterLily;

namespace EmployeeManagementWaterLily.Controllers
{
    public class PublicHolidaysController : Controller
    {
        private EmployeeDBEntities1 db = new EmployeeDBEntities1();

        // GET: PublicHolidays
        public ActionResult Index()
        {
            List<PublicHoliday> publicHolidays;

            // Check if the cache already has the data
            if (HttpContext.Cache["PublicHolidays"] != null)
            {
                // Get data from cache
                publicHolidays = (List<PublicHoliday>)HttpContext.Cache["PublicHolidays"];
            }
            else
            {
                // If not in cache, retrieve from the database and store in cache
                publicHolidays = db.PublicHolidays.ToList();
                HttpContext.Cache.Insert(
                    "PublicHolidays",            // Cache key
                    publicHolidays,              // Cache value
                    null,                        // No cache dependency
                    DateTime.Now.AddMinutes(10), // Cache expiration time
                    System.Web.Caching.Cache.NoSlidingExpiration
                );
            }

            return View(publicHolidays);
        }

        // GET: PublicHolidays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicHoliday publicHoliday = db.PublicHolidays.Find(id);
            if (publicHoliday == null)
            {
                return HttpNotFound();
            }
            return View(publicHoliday);
        }

        // GET: PublicHolidays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicHolidays/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HolidayId,HolidayDate,HolidayName")] PublicHoliday publicHoliday)
        {
            if (ModelState.IsValid)
            {
                db.PublicHolidays.Add(publicHoliday);
                db.SaveChanges();

                // Update the cache after adding a new holiday
                HttpContext.Cache.Remove("PublicHolidays");

                return RedirectToAction("Index");
            }

            return View(publicHoliday);
        }

        // GET: PublicHolidays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicHoliday publicHoliday = db.PublicHolidays.Find(id);
            if (publicHoliday == null)
            {
                return HttpNotFound();
            }
            return View(publicHoliday);
        }

        // POST: PublicHolidays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HolidayId,HolidayDate,HolidayName")] PublicHoliday publicHoliday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publicHoliday).State = EntityState.Modified;
                db.SaveChanges();

                // Update the cache after editing a holiday
                HttpContext.Cache.Remove("PublicHolidays");

                return RedirectToAction("Index");
            }
            return View(publicHoliday);
        }

        // GET: PublicHolidays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicHoliday publicHoliday = db.PublicHolidays.Find(id);
            if (publicHoliday == null)
            {
                return HttpNotFound();
            }
            return View(publicHoliday);
        }

        // POST: PublicHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PublicHoliday publicHoliday = db.PublicHolidays.Find(id);
            db.PublicHolidays.Remove(publicHoliday);
            db.SaveChanges();

            // Update the cache after deleting a holiday
            HttpContext.Cache.Remove("PublicHolidays");

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
