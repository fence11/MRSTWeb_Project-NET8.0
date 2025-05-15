using BigBox_v4.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigBox_v4.Data
{
    public class BoxRepository : Repository<Box>, IBoxRepository
    {
        private readonly ApplicationDBContext _context;

        public BoxRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Box> GetByIdAsync(int id)
        {
            return await _context.Boxes
                .Include(b => b.Driver)
                .Include(b => b.Truck)
                .FirstOrDefaultAsync(b => b.BoxId == id);
        }

        public override async Task<IEnumerable<Box>> GetAllAsync()
        {
            return await _context.Boxes
                .Include(b => b.Driver)
                .Include(b => b.Truck)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Box>> GetBoxesByDriverIdAsync(int driverId)
        {
            return await _context.Boxes
                .Include(b => b.Driver)
                .Include(b => b.Truck)
                .Where(b => b.DriverId == driverId)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Box>> GetBoxesByTruckIdAsync(int truckId)
        {
            return await _context.Boxes
                .Include(b => b.Driver)
                .Include(b => b.Truck)
                .Where(b => b.TruckId == truckId)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Box>> GetBoxesByStatusAsync(string status)
        {
            return await _context.Boxes
                .Include(b => b.Driver)
                .Include(b => b.Truck)
                .Where(b => b.Status == status)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();
        }
    }
}
