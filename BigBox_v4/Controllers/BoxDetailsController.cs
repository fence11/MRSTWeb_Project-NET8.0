using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BigBox_v4.Controllers
{
    public class BoxDetailsController : Controller
    {
        private readonly IBoxBusinessLogic _boxBusinessLogic;
        private readonly IBoxSizeBusinessLogic _boxSizeBusinessLogic;
        private readonly IDriversBusinessLogic _driversBusinessLogic;
        private readonly IDriverScheduleBusinessLogic _scheduleBusinessLogic;
        private readonly IRepository<Truck> _truckRepository;

        public BoxDetailsController(
            IBoxBusinessLogic boxBusinessLogic,
            IBoxSizeBusinessLogic boxSizeBusinessLogic,
            IDriversBusinessLogic driversBusinessLogic,
            IDriverScheduleBusinessLogic scheduleBusinessLogic,
            IRepository<Truck> truckRepository)
        {
            _boxBusinessLogic = boxBusinessLogic;
            _boxSizeBusinessLogic = boxSizeBusinessLogic;
            _driversBusinessLogic = driversBusinessLogic;
            _scheduleBusinessLogic = scheduleBusinessLogic;
            _truckRepository = truckRepository;
        }

        // Step 1: Choose box size and details
        public async Task<IActionResult> Index()
        {
            var boxSizes = await _boxSizeBusinessLogic.GetBoxSizesSortedByVolumeAsync();
            ViewBag.BoxSizes = new SelectList(boxSizes, "BoxSizeId", "Name");
            return View(new Box());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Box box, int boxSizeId)
        {
            if (ModelState.IsValid)
            {
                // Get the selected box size
                var boxSize = await _boxSizeBusinessLogic.GetItemByIdAsync(boxSizeId);
                if (boxSize != null)
                {
                    // Set the dimensions from the selected box size
                    box.Width = boxSize.Width;
                    box.Height = boxSize.Height;
                    box.Length = boxSize.Length;
                }

                // Save the box to session for the next step
                HttpContext.Session.SetString("CurrentBox", JsonSerializer.Serialize(box));

                return RedirectToAction(nameof(SelectDriver));
            }

            var boxSizes = await _boxSizeBusinessLogic.GetBoxSizesSortedByVolumeAsync();
            ViewBag.BoxSizes = new SelectList(boxSizes, "BoxSizeId", "Name");
            return View(box);
        }

        // Step 2: Select a driver
        public async Task<IActionResult> SelectDriver()
        {
            // Get the box from session
            if (!HttpContext.Session.TryGetValue("CurrentBox", out var boxData))
            {
                return RedirectToAction(nameof(Index));
            }

            var box = JsonSerializer.Deserialize<Box>(boxData);

            // Get all drivers with their schedules
            var drivers = await _driversBusinessLogic.GetAllItemsAsync();
            var availableDrivers = drivers.ToList();

            // For each driver, get their upcoming schedules
            foreach (var driver in availableDrivers)
            {
                var schedules = await _scheduleBusinessLogic.GetUpcomingSchedulesAsync(driver.Id);
                ViewData[$"Schedules_{driver.Id}"] = schedules;
            }

            ViewBag.Drivers = availableDrivers;
            return View(box);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectDriver(int driverId)
        {
            // Get the box from session
            if (!HttpContext.Session.TryGetValue("CurrentBox", out var boxData))
            {
                return RedirectToAction(nameof(Index));
            }

            var box = JsonSerializer.Deserialize<Box>(boxData);

            // Assign the selected driver
            box.DriverId = driverId;

            // Update the box in session
            HttpContext.Session.SetString("CurrentBox", JsonSerializer.Serialize(box));

            return RedirectToAction(nameof(SelectTruck));
        }

        // Step 3: Select a truck
        public async Task<IActionResult> SelectTruck()
        {
            // Get the box from session
            if (!HttpContext.Session.TryGetValue("CurrentBox", out var boxData))
            {
                return RedirectToAction(nameof(Index));
            }

            var box = JsonSerializer.Deserialize<Box>(boxData);

            // Get available trucks (Idle or Loading)
            var allTrucks = await _truckRepository.GetAllAsync();
            var availableTrucks = allTrucks.Where(t => t.TruckState == "Idle" || t.TruckState == "Loading").ToList();

            ViewBag.Trucks = availableTrucks;
            return View(box);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectTruck(int truckId)
        {
            // Get the box from session
            if (!HttpContext.Session.TryGetValue("CurrentBox", out var boxData))
            {
                return RedirectToAction(nameof(Index));
            }

            var box = JsonSerializer.Deserialize<Box>(boxData);

            // Assign the selected truck
            box.TruckId = truckId;

            // Update the box in session
            HttpContext.Session.SetString("CurrentBox", JsonSerializer.Serialize(box));

            return RedirectToAction(nameof(Confirmation));
        }

        // Step 4: Confirmation
        public async Task<IActionResult> Confirmation()
        {
            // Get the box from session
            if (!HttpContext.Session.TryGetValue("CurrentBox", out var boxData))
            {
                return RedirectToAction(nameof(Index));
            }

            var box = JsonSerializer.Deserialize<Box>(boxData);

            // Get the driver and truck details
            if (box.DriverId.HasValue)
            {
                ViewBag.Driver = await _driversBusinessLogic.GetItemByIdAsync(box.DriverId.Value);
            }

            if (box.TruckId.HasValue)
            {
                ViewBag.Truck = await _truckRepository.GetByIdAsync(box.TruckId.Value);
            }

            return View(box);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmation(string action)
        {
            // Get the box from session
            if (!HttpContext.Session.TryGetValue("CurrentBox", out var boxData))
            {
                return RedirectToAction(nameof(Index));
            }

            var box = JsonSerializer.Deserialize<Box>(boxData);

            if (action == "Finish")
            {
                // Save the box to the database
                box.Status = "Pending";
                box.CreatedDate = DateTime.Now;
                await _boxBusinessLogic.CreateItemAsync(box);

                // Clear the session
                HttpContext.Session.Remove("CurrentBox");

                // Redirect to the success page
                return RedirectToAction(nameof(Success));
            }
            else if (action == "ChooseAnother")
            {
                // Save the current box
                box.Status = "Pending";
                box.CreatedDate = DateTime.Now;
                await _boxBusinessLogic.CreateItemAsync(box);

                // Clear the session and start a new box
                HttpContext.Session.Remove("CurrentBox");

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Confirmation));
        }

        // Success page
        public IActionResult Success()
        {
            return View();
        }
    }
}
