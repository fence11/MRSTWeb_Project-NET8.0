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

            // Fetch schedules for the given driver ID
            var schedules = await _scheduleRepository.GetSchedulesByDriverIdAsync(driverId);

            // Return the fetched schedules
            return schedules;
        }

        public async Task<IEnumerable<DriverSchedule>> GetSchedulesForWeekAsync(DateTime weekStartDate)
        {
            // `weekStartDate.Date` strips the time portion, therefore startDate starts at 00:00:00 of the specified date.
            var startDate = weekStartDate.Date;
            var endDate = startDate.AddDays(7).AddSeconds(-1); // startdate + 7 days - 1 second (23:59:59 on day 7)

            return await _scheduleRepository.GetSchedulesByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<DriverSchedule>> GetUpcomingSchedulesAsync(int driverId, int daysAhead = 7)
        {
            if (driverId <= 0)
                throw new ArgumentException("Invalid driver ID", nameof(driverId));

            if (daysAhead <= 0)
                throw new ArgumentException("Days ahead must be positive", nameof(daysAhead));

            return await _scheduleRepository.GetUpcomingSchedulesAsync(driverId, daysAhead);
        }

        public async Task<bool> IsScheduleValidAsync(DriverSchedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            if (schedule.StartTime >= schedule.EndTime)
                return false;

            // if schedule is before current date
            if (schedule.StartTime < DateTime.Now)
                return false;

            // if schedules overlap
            bool hasOverlap = await _scheduleRepository.HasOverlappingScheduleAsync(
                schedule.DriverId,
                schedule.StartTime,
                schedule.EndTime,
                schedule.Id > 0 ? schedule.Id : (int?)null);

            return !hasOverlap;
        }

        public override async Task<DriverSchedule> CreateItemAsync(DriverSchedule entity)
        {
            if (!await IsScheduleValidAsync(entity))
                throw new InvalidOperationException("The schedule is invalid or overlaps with an existing schedule");

            // save entity if schedule valid
            return await base.CreateItemAsync(entity);
        }

        public override async Task UpdateItemAsync(DriverSchedule entity)
        {
            if (!await IsScheduleValidAsync(entity))
                throw new InvalidOperationException("The schedule is invalid or overlaps with an existing schedule");

            await base.UpdateItemAsync(entity);
        }
    }
}
