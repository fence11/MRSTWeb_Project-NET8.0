using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BigBox_v4.Data
{
    public class DriverScheduleRepository : Repository<DriverSchedule>, IDriverScheduleRepository
    {
        private readonly ApplicationDBContext _context;

        public DriverScheduleRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<DriverSchedule> GetByIdAsync(int id)
        {
            // Override to include the Driver navigation property
            // when you retrieve a `DriverSchedule` object, its `Driver` property will be `null` unless you explicitly tell EF Core to include it.
            // Load the `DriverSchedule` entity with the specified ID
            // Also load the related `Driver` entity and populate the `Driver` navigation property
            // this is done so that driverName is not null in the view
            return await _context.DriverSchedules
            .Include(s => s.Driver)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<IEnumerable<DriverSchedule>> GetAllAsync()
        {
            // Override to include the Driver navigation property
            return await _context.DriverSchedules
                .Include(s => s.Driver)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<DriverSchedule>> GetSchedulesByDriverIdAsync(int driverId)
        {
            return await _context.DriverSchedules
                .Where(s => s.DriverId == driverId)
                .Include(s => s.Driver)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<DriverSchedule>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.DriverSchedules
                .Where(s => (s.StartTime >= startDate && s.StartTime <= endDate) ||
                            (s.EndTime >= startDate && s.EndTime <= endDate) ||
                            (s.StartTime <= startDate && s.EndTime >= endDate))
                .Include(s => s.Driver)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<DriverSchedule>> GetUpcomingSchedulesAsync(int driverId, int daysAhead)
        {
            var today = DateTime.Today;
            var endDate = today.AddDays(daysAhead);

            return await _context.DriverSchedules
                .Where(s => s.DriverId == driverId &&
                           s.StartTime >= today &&
                           s.StartTime <= endDate)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingScheduleAsync(int driverId, DateTime startTime, DateTime endTime, int? excludeScheduleId = null)
        {
            var query = _context.DriverSchedules
                .Where(s => s.DriverId == driverId &&
                           ((s.StartTime <= startTime && s.EndTime > startTime) ||
                            (s.StartTime < endTime && s.EndTime >= endTime) ||
                            (s.StartTime >= startTime && s.EndTime <= endTime)));

            if (excludeScheduleId.HasValue)
            {
                query = query.Where(s => s.Id != excludeScheduleId.Value);
            }

            return await query.AnyAsync();
        }
    }
}

