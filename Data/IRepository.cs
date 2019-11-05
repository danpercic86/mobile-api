using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Entities;

namespace itec_mobile_api_final.Data
{
    public interface IRepository<T> where T: Entity
    {    
        Task<T> GetAsync<TKey>(TKey id);
        Task<IQueryable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(int id);
        IQueryable<T> Queryable { get; }
    }
}