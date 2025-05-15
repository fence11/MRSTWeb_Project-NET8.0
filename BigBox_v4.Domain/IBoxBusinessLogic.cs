using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IBoxBusinessLogic : IBusinessLogic<Box>
    {
        Task<IEnumerable<Box>> GetBoxesByDriverIdAsync(int driverId);
        Task<IEnumerable<Box>> GetBoxesByTruckIdAsync(int truckId);
        Task<IEnumerable<Box>> GetBoxesByStatusAsync(string status);
        Task<bool> AssignDriverAsync(int boxId, int driverId);
        Task<bool> AssignTruckAsync(int boxId, int truckId);
        Task<bool> UpdateStatusAsync(int boxId, string status);
    }
}
