using BigBox_v4.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.BusinessLogic
{
    public class BoxSizeBusinessLogic : BusinessService<BoxSize>, IBoxSizeBusinessLogic
    {
        private readonly IBoxSizeRepository _boxSizeRepository;

        public BoxSizeBusinessLogic(IBoxSizeRepository repository) : base(repository)
        {
            _boxSizeRepository = repository;
        }

        public async Task<IEnumerable<BoxSize>> GetBoxSizesSortedByVolumeAsync()
        {
            return await _boxSizeRepository.GetBoxSizesSortedByVolumeAsync();
        }
    }
}
