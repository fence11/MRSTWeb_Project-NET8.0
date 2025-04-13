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


    }
}
