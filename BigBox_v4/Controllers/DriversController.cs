using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BigBox_v4.Controllers
{
    public class DriversController : Controller
    {
        private readonly IDriversBusinessLogic _driversBusinessLogic;

        public DriversController(IDriversBusinessLogic driversBusinessLogic)
        {
            _driversBusinessLogic = driversBusinessLogic;
        }

        public async Task<IActionResult> Index()
        {
            var drivers = await _driversBusinessLogic.GetAllItemsAsync();
            return View(drivers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var driver = await _driversBusinessLogic.GetItemByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Drivers driver)
        {
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
            var driver = await _driversBusinessLogic.GetItemByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Drivers driver)
        {
            if (id != driver.Id)
            {
                return NotFound();
            }

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
            var driver = await _driversBusinessLogic.GetItemByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _driversBusinessLogic.DeleteItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
    }
}

