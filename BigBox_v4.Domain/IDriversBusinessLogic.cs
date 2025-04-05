using BigBox_v4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IDriversBusinessLogic : IBusinessLogic<Drivers>
    {
        Task<IEnumerable<Drivers>> GetDriversWithValidLicenseAsync();
        
        Task<bool> IsLicenseNumberUniqueAsync(string licenseNumber);
    }
}

