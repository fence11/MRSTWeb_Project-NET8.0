using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigBox_v4.Domain;

namespace BigBox_v4.BusinessLogic
{
    public class DriverScheduleBusinessLogic : BusinessService<DriverSchedule>, IDriverScheduleBusinessLogic
    {
        private readonly IDriverScheduleRepository _scheduleRepository;

        public DriverScheduleBusinessLogic(IDriverScheduleRepository repository) : base(repository)
        {
            _scheduleRepository = repository;
        }

        public async Task<IEnumerable<DriverSchedule>> GetDriverSchedulesAsync(int driverId)
        {
            if (driverId <= 0)
                throw new ArgumentException("Invalid driver ID", nameof(driverId));

            return await _scheduleRepository.GetSchedulesByDriverIdAsync(driverId);
        }

        public async Task<IEnumerable<DriverSchedule>> GetSchedulesForWeekAsync(DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(7);
            return await _scheduleRepository.GetSchedulesByDateRangeAsync(weekStart, weekEnd);
        }

        public async Task<IEnumerable<DriverSchedule>> GetUpcomingSchedulesAsync(int driverId, int daysAhead)
        {
            return await _scheduleRepository.GetUpcomingSchedulesAsync(driverId, daysAhead);
        }

        public async Task<bool> IsScheduleValidAsync(DriverSchedule schedule)
        {
            if (schedule == null || schedule.DriverId <= 0 || schedule.StartTime >= schedule.EndTime)
                return false;

            return !await _scheduleRepository.HasOverlappingScheduleAsync(
                schedule.DriverId,
                schedule.StartTime,
                schedule.EndTime,
                schedule.Id == 0 ? null : (int?)schedule.Id
            );
        }
    }
}
