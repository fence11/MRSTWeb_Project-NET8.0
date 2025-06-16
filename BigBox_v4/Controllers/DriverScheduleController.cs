using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BigBox_v4.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class DriverScheduleController : Controller
    {
        private readonly IDriverScheduleBusinessLogic _scheduleBusinessLogic;
        private readonly IDriversBusinessLogic _driversBusinessLogic;

        public DriverScheduleController(
            IDriverScheduleBusinessLogic scheduleBusinessLogic,
            IDriversBusinessLogic driversBusinessLogic)
        {
            _scheduleBusinessLogic = scheduleBusinessLogic;
            _driversBusinessLogic = driversBusinessLogic;
        }

        // GET: DriverSchedule
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var schedules = await _scheduleBusinessLogic.GetSchedulesForWeekAsync(today);
            return View(schedules);
        }

        // GET: DriverSchedule/DriverTimetable/ID
        public async Task<IActionResult> DriverTimetable(int id)
        {
            var driver = await _driversBusinessLogic.GetItemByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            var schedules = await _scheduleBusinessLogic.GetDriverSchedulesAsync(id);
            ViewBag.Driver = driver;
            return View(schedules);
        }

        // GET: DriverSchedule/Create
        public async Task<IActionResult> Create(int? driverId)
        {
            if (!UserIsAdmin()) return Forbid();
            await PopulateDriversDropdown();

            var schedule = new DriverSchedule
            {
                StartTime = DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1),
                EndTime = DateTime.Now.Date.AddHours(DateTime.Now.Hour + 9),
                Status = "Scheduled"
            };

            if (driverId.HasValue)
            {
                schedule.DriverId = driverId.Value;
            }

            return View(schedule);
        }

        // POST: DriverSchedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DriverSchedule schedule)
        {
            if (!UserIsAdmin()) return Forbid();
            if (ModelState.IsValid)
            {
                try
                {
                    await _scheduleBusinessLogic.CreateItemAsync(schedule);

                    if (Request.Query.ContainsKey("returnToDriver"))
                    {
                        return RedirectToAction("DriverTimetable", new { id = schedule.DriverId });
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await PopulateDriversDropdown();
            return View(schedule);
        }

        // GET: DriverSchedule/Edit/ID
        public async Task<IActionResult> Edit(int id)
        {
            if (!UserIsAdmin()) return Forbid();
            var schedule = await _scheduleBusinessLogic.GetItemByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            await PopulateDriversDropdown();
            return View(schedule);
        }

        // POST: DriverSchedule/Edit/ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DriverSchedule schedule)
        {
            if (!UserIsAdmin()) return Forbid();
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _scheduleBusinessLogic.UpdateItemAsync(schedule);

                    if (Request.Query.ContainsKey("returnToDriver"))
                    {
                        return RedirectToAction("DriverTimetable", new { id = schedule.DriverId });
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await PopulateDriversDropdown();
            return View(schedule);
        }

        // GET: DriverSchedule/Delete/ID
        public async Task<IActionResult> Delete(int id)
        {
            if (!UserIsAdmin()) return Forbid();
            var schedule = await _scheduleBusinessLogic.GetItemByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: DriverSchedule/Delete/ID
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!UserIsAdmin()) return Forbid();
            var schedule = await _scheduleBusinessLogic.GetItemByIdAsync(id);
            int driverId = schedule.DriverId;

            await _scheduleBusinessLogic.DeleteItemAsync(id);

            if (Request.Query.ContainsKey("returnToDriver"))
            {
                return RedirectToAction("DriverTimetable", new { id = driverId });
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDriversDropdown()
        {
            var drivers = await _driversBusinessLogic.GetAllItemsAsync();
            ViewBag.Drivers = new SelectList(drivers, "Id", "Name");
        }

        private bool UserIsAdmin()
        {
            return User.HasClaim("IsAdmin", "True");
        }
    }
}
