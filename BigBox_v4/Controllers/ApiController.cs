using BigBox_v4.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BigBox_v4.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IRepository<Truck> _truckRepository;

        public ApiController(IRepository<Truck> truckRepository)
        {
            _truckRepository = truckRepository;
        }

        [HttpPost("trucks/{id}/status")]
        public async Task<IActionResult> UpdateTruckStatus(int id, [FromBody] UpdateTruckStatusRequest request)
        {
            if (string.IsNullOrEmpty(request.Status))
            {
                return BadRequest("Status cannot be empty");
            }

            var truck = await _truckRepository.GetByIdAsync(id);
            if (truck == null)
            {
                return NotFound();
            }

            truck.TruckState = request.Status;
            await _truckRepository.UpdateAsync(truck);

            return Ok();
        }
    }

    public class UpdateTruckStatusRequest
    {
        public string Status { get; set; }
    }
}
