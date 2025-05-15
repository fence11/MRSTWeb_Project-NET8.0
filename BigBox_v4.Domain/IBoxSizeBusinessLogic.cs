using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IBoxSizeBusinessLogic : IBusinessLogic<BoxSize>
    {
        Task<IEnumerable<BoxSize>> GetBoxSizesSortedByVolumeAsync();
    }
}
