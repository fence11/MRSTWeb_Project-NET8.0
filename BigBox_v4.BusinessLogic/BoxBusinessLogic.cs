using BigBox_v4.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.BusinessLogic
{
    public class BoxBusinessLogic : BusinessService<Box>, IBoxBusinessLogic
    {
        private readonly IBoxRepository _boxRepository;

        public BoxBusinessLogic(IBoxRepository repository) : base(repository)
        {
            _boxRepository = repository;
        }

        public async Task<IEnumerable<Box>> GetBoxesByDriverIdAsync(int driverId)
        {
            if (driverId <= 0)
                throw new ArgumentException("Invalid driver ID", nameof(driverId));

            return await _boxRepository.GetBoxesByDriverIdAsync(driverId);
        }

        public async Task<IEnumerable<Box>> GetBoxesByTruckIdAsync(int truckId)
        {
            if (truckId <= 0)
                throw new ArgumentException("Invalid truck ID", nameof(truckId));

            return await _boxRepository.GetBoxesByTruckIdAsync(truckId);
        }

        public async Task<IEnumerable<Box>> GetBoxesByStatusAsync(string status)
        {
            if (string.IsNullOrEmpty(status))
                throw new ArgumentException("Status cannot be empty", nameof(status));

            return await _boxRepository.GetBoxesByStatusAsync(status);
        }

        public async Task<bool> AssignDriverAsync(int boxId, int driverId)
        {
            if (boxId <= 0)
                throw new ArgumentException("Invalid box ID", nameof(boxId));

            if (driverId <= 0)
                throw new ArgumentException("Invalid driver ID", nameof(driverId));

            var box = await _boxRepository.GetByIdAsync(boxId);
            if (box == null)
                return false;

            box.DriverId = driverId;
            await _boxRepository.UpdateAsync(box);
            return true;
        }

        public async Task<bool> AssignTruckAsync(int boxId, int truckId)
        {
            if (boxId <= 0)
                throw new ArgumentException("Invalid box ID", nameof(boxId));

            if (truckId <= 0)
                throw new ArgumentException("Invalid truck ID", nameof(truckId));

            var box = await _boxRepository.GetByIdAsync(boxId);
            if (box == null)
                return false;

            box.TruckId = truckId;
            await _boxRepository.UpdateAsync(box);
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int boxId, string status)
        {
            if (boxId <= 0)
                throw new ArgumentException("Invalid box ID", nameof(boxId));

            if (string.IsNullOrEmpty(status))
                throw new ArgumentException("Status cannot be empty", nameof(status));

            var box = await _boxRepository.GetByIdAsync(boxId);
            if (box == null)
                return false;

            box.Status = status;
            await _boxRepository.UpdateAsync(box);
            return true;
        }
    }
}
