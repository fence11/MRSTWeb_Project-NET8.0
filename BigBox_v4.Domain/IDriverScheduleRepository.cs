using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IDriverScheduleRepository : IRepository<DriverSchedule>
    {
        Task<IEnumerable<DriverSchedule>> GetSchedulesByDriverIdAsync(int driverId);
        Task<IEnumerable<DriverSchedule>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DriverSchedule>> GetUpcomingSchedulesAsync(int driverId, int daysAhead);
        Task<bool> HasOverlappingScheduleAsync(int driverId, DateTime startTime, DateTime endTime, int? excludeScheduleId = null);
    }
}
