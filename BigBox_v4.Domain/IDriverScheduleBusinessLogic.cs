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
    }
}
