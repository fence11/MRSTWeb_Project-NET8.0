using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BigBox_v4.Controllers
{
     [Authorize]
     public class DriversController : Controller
     {
          private readonly IDriversBusinessLogic _driversBusinessLogic;
          private readonly IDriverScheduleBusinessLogic _scheduleBusinessLogic;

          public DriversController(
              IDriversBusinessLogic driversBusinessLogic,
              IDriverScheduleBusinessLogic scheduleBusinessLogic)
          {
               _driversBusinessLogic = driversBusinessLogic;
               _scheduleBusinessLogic = scheduleBusinessLogic;
          }

          public async Task<IActionResult> Index()
          {
               if (!UserIsAdmin()) return Forbid();
               var drivers = await _driversBusinessLogic.GetAllItemsAsync();
               return View(drivers);
          }

          public async Task<IActionResult> Details(int id)
          {
               if (!UserIsAdmin()) return Forbid();
               var driver = await _driversBusinessLogic.GetItemByIdAsync(id);
               if (driver == null) return NotFound();

               ViewBag.UpcomingSchedules = await _scheduleBusinessLogic.GetUpcomingSchedulesAsync(id);
               return View(driver);
          }

          public IActionResult Create()
          {
               if (!UserIsAdmin()) return Forbid();
               return View();
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create(Drivers driver)
          {
               if (!UserIsAdmin()) return Forbid();

               if (ModelState.IsValid)
               {
                    try
                    {
                         await _driversBusinessLogic.CreateItemAsync(driver);
                         return RedirectToAction(nameof(Index));
                    }
                    catch (InvalidOperationException ex)
                    {
                         ModelState.AddModelError("", ex.Message);
                    }
               }
               return View(driver);
          }

          public async Task<IActionResult> Edit(int id)
          {
               if (!UserIsAdmin()) return Forbid();
               var driver = await _driversBusinessLogic.GetItemByIdAsync(id);
               if (driver == null) return NotFound();
               return View(driver);
          }

          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Edit(int id, Drivers driver)
          {
               if (!UserIsAdmin()) return Forbid();
               if (id != driver.Id) return NotFound();

               if (ModelState.IsValid)
               {
                    try
                    {
                         await _driversBusinessLogic.UpdateItemAsync(driver);
                         return RedirectToAction(nameof(Index));
                    }
                    catch (InvalidOperationException ex)
                    {
                         ModelState.AddModelError("", ex.Message);
                    }
               }
               return View(driver);
          }

          public async Task<IActionResult> Delete(int id)
          {
               if (!UserIsAdmin()) return Forbid();
               var driver = await _driversBusinessLogic.GetItemByIdAsync(id);
               if (driver == null) return NotFound();
               return View(driver);
          }

          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(int id)
          {
               if (!UserIsAdmin()) return Forbid();
               await _driversBusinessLogic.DeleteItemAsync(id);
               return RedirectToAction(nameof(Index));
          }

          private bool UserIsAdmin()
          {
               return User.HasClaim("IsAdmin", "True");
          }
     }
}
