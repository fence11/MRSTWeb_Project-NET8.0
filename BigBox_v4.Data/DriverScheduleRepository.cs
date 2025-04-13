using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigBox_v4.Domain;
using Microsoft.EntityFrameworkCore;

namespace BigBox_v4.Data
{
    public class DriverScheduleRepository : Repository<DriverSchedule>, IDriverScheduleRepository
    {
        private readonly ApplicationDBContext _context;

        public DriverScheduleRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DriverSchedule>> GetSchedulesByDriverIdAsync(int driverId)
        {
            return await _context.DriverSchedules
                .Where(s => s.DriverId == driverId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }
    }
}
