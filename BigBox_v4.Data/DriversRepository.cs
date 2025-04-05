using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigBox_v4.Data
{
    public class DriversRepository : Repository<Drivers>, IDriversRepository
    {
        private readonly ApplicationDBContext _context;

        public DriversRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drivers>> GetDriversByEmploymentYearAsync(int year)
        {
            return await _context.Set<Drivers>()
                .Where(d => d.EmploymentDate.Year == year)
                .ToListAsync();
        }

        public async Task<Drivers?> GetDriverByLicenseNumberAsync(string licenseNumber)
        {
            return await _context.Set<Drivers>()
                .FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber);
        }

        public async Task<bool> DriverExistsAsync(string licenseNumber)
        {
            return await _context.Set<Drivers>().AnyAsync(d => d.LicenseNumber == licenseNumber);
        }
    }
}

