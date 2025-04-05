using BigBox_v4.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBox_v4.BusinessLogic
{
    public class BusinessService<T> : IBusinessLogic<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public BusinessService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAllItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<T> GetItemByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID", nameof(id));

            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<T> CreateItemAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return await _repository.AddAsync(entity);
        }

        public virtual async Task UpdateItemAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _repository.UpdateAsync(entity);
        }

        public virtual async Task DeleteItemAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID", nameof(id));

            await _repository.DeleteAsync(id);
        }
    }
}

