using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IDriverScheduleBusinessLogic : IBusinessLogic<DriverSchedule>
    {
        Task<IEnumerable<DriverSchedule>> GetDriverSchedulesAsync(int driverId);
        Task<IEnumerable<DriverSchedule>> GetSchedulesForWeekAsync(DateTime weekStartDate);
        Task<IEnumerable<DriverSchedule>> GetUpcomingSchedulesAsync(int driverId, int daysAhead = 7);
        Task<bool> IsScheduleValidAsync(DriverSchedule schedule);
    }
}

