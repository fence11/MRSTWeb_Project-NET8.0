using BigBox_v4.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigBox_v4.Data
{
    public class BoxSizeRepository : Repository<BoxSize>, IBoxSizeRepository
    {
        private readonly ApplicationDBContext _context;

        public BoxSizeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BoxSize>> GetBoxSizesSortedByVolumeAsync()
        {
            return await _context.BoxSizes
                .OrderBy(b => b.Width * b.Height * b.Length)
                .ToListAsync();
        }
    }
}
