using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BigBox_v4.Controllers
{
    public class DriverScheduleController : Controller
    {
        private readonly IDriverScheduleBusinessLogic _scheduleBusinessLogic;

        public DriverScheduleController(IDriverScheduleBusinessLogic scheduleBusinessLogic)
        {
            _scheduleBusinessLogic = scheduleBusinessLogic;
        }

        public async Task<IActionResult> Index()
        {
            var schedules = await _scheduleBusinessLogic.GetAllItemsAsync();
            return View(schedules);
        }
    }
}
