using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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

            return RedirectToAction(nameof(LoadPlanning));
        }

        // Step 4: Load Planning
        public async Task<IActionResult> LoadPlanning()
        {
            // Get the box from session
            if (!HttpContext.Session.TryGetValue("CurrentBox", out var boxData))
            {
                return RedirectToAction(nameof(Index));
            }

            var box = JsonSerializer.Deserialize<Box>(boxData);

            if (!box.TruckId.HasValue)
            {
                return RedirectToAction(nameof(SelectTruck));
            }

            // Get the truck details
            var truck = await _truckRepository.GetByIdAsync(box.TruckId.Value);
            if (truck == null)
            {
                return RedirectToAction(nameof(SelectTruck));
            }

            // Get existing boxes for this truck
            var existingBoxes = await _boxBusinessLogic.GetBoxesByTruckIdAsync(truck.TruckID);

            // Calculate how many boxes can fit in the truck
            var loadPlanningResult = CalculateLoadPlanning(box, truck, existingBoxes.ToList());

            ViewBag.Truck = truck;
            ViewBag.ExistingBoxes = existingBoxes;
            ViewBag.LoadPlanningResult = loadPlanningResult;

            return View(box);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadPlanning(string action)
        {
            // Get the box from session
            if (!HttpContext.Session.TryGetValue("CurrentBox", out var boxData))
            {
                return RedirectToAction(nameof(Index));
            }

            var box = JsonSerializer.Deserialize<Box>(boxData);

            if (!box.TruckId.HasValue)
            {
                return RedirectToAction(nameof(SelectTruck));
            }

            // Get the truck details
            var truck = await _truckRepository.GetByIdAsync(box.TruckId.Value);
            if (truck == null)
            {
                return RedirectToAction(nameof(SelectTruck));
            }

            if (action == "ConfirmLoad")
            {
                // Update truck status if needed
                if (truck.TruckState == "Idle")
                {
                    truck.TruckState = "Loading";
                    await _truckRepository.UpdateAsync(truck);
                }

                // Save the box to the database
                box.Status = "Loaded";
                box.CreatedDate = DateTime.Now;
                await _boxBusinessLogic.CreateItemAsync(box);

                // Get existing boxes for this truck
                var existingBoxes = await _boxBusinessLogic.GetBoxesByTruckIdAsync(truck.TruckID);

                // If more than 10 boxes are loaded, change truck status to InTransit
                if (existingBoxes.Count() >= 10)
                {
                    truck.TruckState = "InTransit";
                    await _truckRepository.UpdateAsync(truck);

                    // Set a flag in TempData to indicate the truck is now in transit
                    TempData["TruckInTransit"] = true;
                    TempData["TruckId"] = truck.TruckID;
                }

                // Clear the session
                HttpContext.Session.Remove("CurrentBox");

                return RedirectToAction(nameof(Confirmation));
            }
            else if (action == "ChooseAnother")
            {
                // Update truck status if needed
                if (truck.TruckState == "Idle")
                {
                    truck.TruckState = "Loading";
                    await _truckRepository.UpdateAsync(truck);
                }

                // Save the current box
                box.Status = "Loaded";
                box.CreatedDate = DateTime.Now;
                await _boxBusinessLogic.CreateItemAsync(box);

                // Clear the session and start a new box
                HttpContext.Session.Remove("CurrentBox");

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(LoadPlanning));
        }

        // Step 5: Confirmation
        public async Task<IActionResult> Confirmation()
        {
            // Check if a truck has just been set to InTransit
            if (TempData.ContainsKey("TruckInTransit") && TempData.ContainsKey("TruckId"))
            {
                ViewBag.TruckInTransit = true;
                int truckId = (int)TempData["TruckId"];
                ViewBag.Truck = await _truckRepository.GetByIdAsync(truckId);
            }

            return View();
        }

        // Success page
        public IActionResult Success()
        {
            return View();
        }

        // Helper method to calculate load planning
        private LoadPlanningResult CalculateLoadPlanning(Box newBox, Truck truck, List<Box> existingBoxes)
        {
            var result = new LoadPlanningResult();

            // Calculate total volume of the truck
            decimal truckVolume = truck.TowWidth.GetValueOrDefault() * truck.TowHeight.GetValueOrDefault() * truck.TowLength.GetValueOrDefault();
            result.TruckVolume = truckVolume;

            // Calculate volume of the new box
            decimal newBoxVolume = newBox.Width * newBox.Height * newBox.Length;
            result.NewBoxVolume = newBoxVolume;

            // Calculate total volume of existing boxes
            decimal existingBoxesVolume = 0;
            decimal existingBoxesWeight = 0;

            foreach (var box in existingBoxes)
            {
                existingBoxesVolume += box.Width * box.Height * box.Length;
                existingBoxesWeight += box.Weight;
            }

            result.ExistingBoxesVolume = existingBoxesVolume;
            result.ExistingBoxesWeight = existingBoxesWeight;

            // Calculate remaining volume
            result.RemainingVolume = truckVolume - existingBoxesVolume;

            // Calculate how many more boxes like this one can fit
            if (newBoxVolume > 0)
            {
                result.MaxAdditionalBoxes = Math.Floor(result.RemainingVolume / newBoxVolume);
            }

            // Check weight constraints
            result.TruckMaxWeight = truck.MaxLoadCapacity.GetValueOrDefault();
            result.NewBoxWeight = newBox.Weight;
            result.RemainingWeight = result.TruckMaxWeight - existingBoxesWeight;

            if (result.RemainingWeight < newBox.Weight)
            {
                result.WeightExceeded = true;
                result.MaxAdditionalBoxes = 0;
            }

            // Check special handling requirements
            result.SpecialHandlingRequired = newBox.ThisWayUp || newBox.Fragile || newBox.HandleWithCare ||
                                            newBox.KeepDry || newBox.KeepUpright || newBox.Perishable ||
                                            newBox.DoNotStack || newBox.Flammable || newBox.Explosive;

            // Adjust max boxes based on special handling
            if (newBox.DoNotStack || newBox.Fragile)
            {
                // If box can't be stacked, we need to consider floor space only
                decimal floorArea = truck.TowWidth.GetValueOrDefault() * truck.TowLength.GetValueOrDefault();
                decimal boxArea = newBox.Width * newBox.Length;
                decimal remainingFloorArea = floorArea - (existingBoxesVolume / truck.TowHeight.GetValueOrDefault());

                decimal maxBoxesByFloorArea = Math.Floor(remainingFloorArea / boxArea);
                result.MaxAdditionalBoxes = Math.Min(result.MaxAdditionalBoxes, maxBoxesByFloorArea);
            }

            // Check if the truck is already full
            result.TruckIsFull = result.MaxAdditionalBoxes <= 0;

            // Check if this would be the 10th or more box
            result.WillTriggerTransit = (existingBoxes.Count + 1) >= 10;

            return result;
        }
    }

    // Class to hold load planning calculation results
    public class LoadPlanningResult
    {
        public decimal TruckVolume { get; set; }
        public decimal NewBoxVolume { get; set; }
        public decimal ExistingBoxesVolume { get; set; }
        public decimal RemainingVolume { get; set; }
        public decimal MaxAdditionalBoxes { get; set; }

        public decimal TruckMaxWeight { get; set; }
        public decimal NewBoxWeight { get; set; }
        public decimal ExistingBoxesWeight { get; set; }
        public decimal RemainingWeight { get; set; }
        public bool WeightExceeded { get; set; }

        public bool SpecialHandlingRequired { get; set; }
        public bool TruckIsFull { get; set; }
        public bool WillTriggerTransit { get; set; }
    }
}
