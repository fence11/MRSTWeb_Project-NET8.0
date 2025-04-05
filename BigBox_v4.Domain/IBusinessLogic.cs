using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.Domain
{
    public interface IBusinessLogic<T> where T : class
    {
        Task<IEnumerable<T>> GetAllItemsAsync();
        Task<T> GetItemByIdAsync(int id);
        Task<T> CreateItemAsync(T entity);
        Task UpdateItemAsync(T entity);
        Task DeleteItemAsync(int id);
    }
}

