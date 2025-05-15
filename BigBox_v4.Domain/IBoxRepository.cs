using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IBoxRepository : IRepository<Box>
    {
        Task<IEnumerable<Box>> GetBoxesByDriverIdAsync(int driverId);
        Task<IEnumerable<Box>> GetBoxesByTruckIdAsync(int truckId);
        Task<IEnumerable<Box>> GetBoxesByStatusAsync(string status);
    }
}
