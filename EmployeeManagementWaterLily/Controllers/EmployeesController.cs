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
    public class EmployeesController : Controller
    {
        private EmployeeDBEntities1 db = new EmployeeDBEntities1();

        // GET: Employees
        public ActionResult Index()
        {
            List<Employee> employees;

            // Check if the cache already has the data
            if (HttpContext.Cache["Employees"] != null)
            {
                employees = (List<Employee>)HttpContext.Cache["Employees"];
            }
            else
            {
                // If not in cache, retrieve from database and store in cache
                employees = db.Employees.ToList();
                HttpContext.Cache.Insert(
                    "Employees",               // Cache key
                    employees,                 // Cache value
                    null,                      // No cache dependency
                    DateTime.Now.AddMinutes(10), // Cache expiration time
                    System.Web.Caching.Cache.NoSlidingExpiration
                );
            }

            return View(employees);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,JobPosition")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                // Clear cache after adding new data
                HttpContext.Cache.Remove("Employees");
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,JobPosition")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                // Clear cache after modifying data
                HttpContext.Cache.Remove("Employees");
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            // Clear cache after deleting data
            HttpContext.Cache.Remove("Employees");
            return RedirectToAction("Index");
        }

        // GET: Employee/CalculateWorkingDays
        public ActionResult CalculateWorkingDays()
        {
            return View();
        }

        // POST: Employee/CalculateWorkingDays
        [HttpPost]
        public ActionResult CalculateWorkingDays(DateTime startDate, DateTime endDate)
        {
            if (!IsWeekday(startDate))
            {
                ModelState.AddModelError("startDate", "Start date must be a weekday.");
                return View();
            }

            int workingDays = CalculateWorkingDaysBetweenDates(startDate, endDate);
            ViewBag.WorkingDays = workingDays;
            return View();
        }

        // Helper Method: Check if a date is a weekday
        private bool IsWeekday(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        // Helper Method: Retrieve public holidays from the database
        private List<DateTime> GetPublicHolidays()
        {
            // Check if public holidays are cached
            if (HttpContext.Cache["PublicHolidays"] == null)
            {
                var publicHolidays = db.PublicHolidays.Select(h => h.HolidayDate).ToList();
                HttpContext.Cache.Insert(
                    "PublicHolidays",
                    publicHolidays,
                    null,
                    DateTime.Now.AddHours(1),
                    System.Web.Caching.Cache.NoSlidingExpiration
                );
            }
            return (List<DateTime>)HttpContext.Cache["PublicHolidays"];
        }

        // Helper Method: Calculate working days between two dates
        private int CalculateWorkingDaysBetweenDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> publicHolidays = GetPublicHolidays();
            int workingDays = 0;

            // Loop through each date from startDate to endDate (inclusive)
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Check if the date is a weekday and not a public holiday
                if (IsWeekday(date) && !publicHolidays.Contains(date))
                {
                    workingDays++;
                }
            }

            return workingDays;
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
