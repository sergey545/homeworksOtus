using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T : BaseEntity
    {
        protected ICollection<T> Data { get; set; }

        public InMemoryRepository(ICollection<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data.AsEnumerable());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }
        public bool Delete(Guid id)
        {
            if (GetByIdAsync(id).Result != default)
            {
                Data.Remove(GetByIdAsync(id).Result);
                return true;
            }
            else
            {
                return false;
            }
        }
        public Task<T> CreateAsync(T entity)
        {
            Data.Add(entity);
            return Task.FromResult(entity);
        }
        public Task<T> UpdateAsync(T entity)
        {
            Data.Remove(GetByIdAsync(entity.Id).Result);
            Data.Add(entity);
            return Task.FromResult(entity);
        }
    }
}