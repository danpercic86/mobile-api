using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Entities;

namespace itec_mobile_api_final.Data
{
    public interface IRepository<T> where T: Entity
    {    
        T Get<TKey>(TKey id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        IQueryable<T> Queryable { get; }
    }
}