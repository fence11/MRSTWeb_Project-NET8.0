using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IBoxSizeRepository : IRepository<BoxSize>
    {
        Task<IEnumerable<BoxSize>> GetBoxSizesSortedByVolumeAsync();
    }
}
