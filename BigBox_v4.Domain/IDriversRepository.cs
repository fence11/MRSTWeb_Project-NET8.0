using BigBox_v4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IDriversRepository : IRepository<Drivers>
    {
        Task<IEnumerable<Drivers>> GetDriversByEmploymentYearAsync(int year);
        Task<Drivers> GetDriverByLicenseNumberAsync(string licenseNumber);
    }
}

