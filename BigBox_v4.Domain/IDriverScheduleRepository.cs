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
    }
}

